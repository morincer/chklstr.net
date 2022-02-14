using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chklstr.Core.Model;
using Chklstr.Core.Services;
using Chklstr.Core.Tests;
using Chklstr.Infra.Parser;
using Chklstr.UI.Core.Services;
using Chklstr.UI.Core.ViewModels;
using Moq;
using MvvmCross.Navigation;
using MvvmCross.Tests;
using MvvmCross.ViewModels;
using NUnit.Framework;
using Serilog;
using Serilog.Core;

namespace Chklstr.UI.Core.Tests;

public class QRHViewModelTest : MvxIoCSupportingTest
{
    private QRHViewModel _viewModel;
    private Mock<IMvxNavigationService> _navigationServiceMock;
    private Mock<IErrorReporter> _errorReporterMock;

    [SetUp]
    public new async Task Setup()
    {
        base.Setup();
        var parserService = new QRHParserService(TestData.Logger<QRHParserService>());
        var parseResult = parserService.parse(TestData.readFA50Sample());
        Assert.True(parseResult.IsSuccess());
        Assert.NotNull(parseResult.Result);

        _navigationServiceMock = new Mock<IMvxNavigationService>();
        _errorReporterMock = new Mock<IErrorReporter>();

        _viewModel = new QRHViewModel(
            new InMemoryUserSettings(),
            _navigationServiceMock.Object, _errorReporterMock.Object,
            TestData.Logger<QRHViewModel>());
        _viewModel.Prepare(parseResult.Result!);
        await _viewModel.Initialize();
        _viewModel.Start();
    }

    [Test]
    public void ShouldGenerateChecklists()
    {
        Assert.That(_viewModel.Checklists, Is.Not.Empty);
        var cl = _viewModel.Checklists[0];
        Assert.That(cl.Name, Is.EqualTo("Before Engine Start (Power Off)"));
    }

    [Test]
    public void ShouldSetDefaultContextsAndUpdateCounters()
    {
        Assert.That(_viewModel.Contexts, Is.Not.Empty);
        Assert.That(
            _viewModel.Contexts.Where(c => c.Selected).Select(c => c.Name),
            Is.EquivalentTo(_viewModel.Item.DefaultContexts)
        );

        Assert.That(_viewModel.Checklists[0].CheckableItemsCount, Is.EqualTo(0));
        Assert.That(_viewModel.Checklists[0].CheckedItemsCount, Is.EqualTo(0));
        Assert.That(_viewModel.Checklists[1].CheckableItemsCount, Is.GreaterThan(0));
        Assert.That(_viewModel.Checklists[1].CheckedItemsCount, Is.EqualTo(0));
    }

    [Test]
    public void ShouldSelectFirstActiveChecklist()
    {
        Assert.NotNull(_viewModel.SelectedChecklist);
        Assert.That(_viewModel.SelectedChecklist!.Name, Is.EqualTo(_viewModel.Checklists[1].Name));
    }

    [Test]
    public void ShouldUpdateCountersWhenItemChecked()
    {
        var checklistViewModel = _viewModel.Checklists[1];
        Assert.True(checklistViewModel.IsEnabled);
        Assert.That(checklistViewModel.CheckedItemsCount, Is.EqualTo(0));
        Log.Logger.Debug($"Setting checked on {checklistViewModel.Name}/{checklistViewModel.Children[1].Title}");
        checklistViewModel.Children[1].IsChecked = true;
        Assert.That(checklistViewModel.CheckedItemsCount, Is.EqualTo(1));
    }

    [Test]
    public void ShouldUpdateCountersWhenContextsChange()
    {
        var cheklistViewModel = _viewModel.Checklists[1];
        Assert.True(cheklistViewModel.IsEnabled);

        var checkable = cheklistViewModel.CheckableItemsCount;
        Assert.That(checkable, Is.GreaterThan(10));

        foreach (var ctx in _viewModel.Contexts)
        {
            ctx.Selected = false;
        }

        Assert.That(cheklistViewModel.CheckableItemsCount, Is.Not.EqualTo(checkable));
    }

    [Test]
    public void ShouldAssignListNumber()
    {
        var cheklistViewModel = _viewModel.Checklists[1];
        Assert.That(cheklistViewModel.ListNumber, Is.EqualTo("2"));
    }

    [Test]
    public void ShouldCloseWithRedirectionWhenAnotherFileIsOpened()
    {
        var parseResult = ParseResult<QuickReferenceHandbook>.Success(_viewModel.Item);

        _navigationServiceMock.Setup(s =>
                s.Navigate<QRHParsingViewModel, string, ParseResult<QuickReferenceHandbook>>(It.IsAny<string>(),
                    default, default))
            .ReturnsAsync(parseResult);

        QRHViewModelResult? viewModelResult = null;
        
        /*
         * Task<bool> Close<TResult>(IMvxViewModelResult<TResult> viewModel, TResult? result, CancellationToken cancellationToken = default)
            where TResult : class;
         */
        _navigationServiceMock.Setup(s => s.Close<QRHViewModelResult>(
                It.IsAny<IMvxViewModelResult<QRHViewModelResult>>(),
                It.IsAny<QRHViewModelResult?>(),
                default))
            .Callback<IMvxViewModelResult<QRHViewModelResult>, QRHViewModelResult?, CancellationToken>(
                (result, modelResult, token) =>
                {
                    viewModelResult = modelResult;
                })
            .ReturnsAsync(true);

        _viewModel.SelectFilePathInteraction.WeakSubscribe(request =>
        {
            request.FileSelectedCallback("c:/temp/somefile.chklst");
        });
        
        _viewModel.OpenCommand.Execute();
        
        _navigationServiceMock.Verify();
        
        Assert.That(viewModelResult, Is.Not.Null);
        Assert.That(viewModelResult!.RedirectTo, Is.EqualTo(_viewModel.Item));
    }
}