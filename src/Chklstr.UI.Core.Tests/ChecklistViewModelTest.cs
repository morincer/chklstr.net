using System;
using System.Linq;
using System.Threading.Tasks;
using Chklstr.Core.Model;
using Chklstr.UI.Core.ViewModels;
using MvvmCross.Tests;
using NUnit.Framework;
using Serilog;

namespace Chklstr.UI.Core.Tests;

public class ChecklistViewModelTest : MvxIoCSupportingTest
{
    private ChecklistViewModel _checklist;
    
    [SetUp]
    public async Task Setup()
    {
        base.Setup();
        var book = new QuickReferenceHandbook("Test");
        var cl = book.Add("Test List");
        cl.AddSingleItem("Item", "Check");
        cl.AddSingleItem("Item2", "Check");
        
        _checklist = new ChecklistViewModel();
        _checklist.Prepare(cl);
        await _checklist.Initialize();
    }

    [Test]
    public void ShouldSetCounters()
    {
        Assert.That(_checklist.CheckableItemsCount, Is.EqualTo(2));
        Assert.That(_checklist.CheckedItemsCount, Is.EqualTo(0));
        Assert.That(_checklist.IsEnabled, Is.True);
    }
    
    [Test]
    public void ShouldUpdateCountersWithRespectToContexts()
    {
        _checklist.Item.Items[0].Contexts.Add("ctx");
        _checklist.Item.Items[1].Contexts.Add("ctx");
        _checklist.Update();
        
        Assert.That(_checklist.CheckableItemsCount, Is.EqualTo(0));
        Assert.That(_checklist.IsEnabled, Is.False);

        _checklist.Contexts = new[] {"ctx"};

        Assert.That(_checklist.CheckableItemsCount, Is.EqualTo(2));
        Assert.That(_checklist.IsEnabled, Is.True);
    }

    [Test]
    public void ShouldCreateChildrenWithListNumbers()
    {
        Assert.That(_checklist.Children, Is.Not.Empty);
        Assert.That(_checklist.Children[0].ListNumber, Is.EqualTo("1"));
    }

    [Test]
    public void ShouldEnableItemsBasingOnContext()
    {
        foreach (var item in _checklist.Children)
        {
            Assert.That(item.IsEnabled, Is.EqualTo(item.Item.IsAvailableInContext(_checklist.Contexts)));
        }

        _checklist.Contexts = new string[] {"ctx"};
        
        foreach (var item in _checklist.Children)
        {
            Assert.That(item.IsEnabled, Is.EqualTo(item.Item.IsAvailableInContext(_checklist.Contexts)));
        }
        
    }

    [Test]
    public void ShouldSelectFirstActiveItemUponInitialization()
    {
        Assert.NotNull(_checklist.SelectedItem);
        Assert.That(_checklist.SelectedItem?.Title, Is.EqualTo("Item"));
    }

    [Test]
    public void ShouldBeMarkedCompleteOnceAllItemsAreComplete()
    {
        foreach (var item in _checklist.Children.Where(c => c.IsEnabled))
        {
            item.IsChecked = true;
        }
        
        Assert.True(_checklist.IsComplete);
    }

    [Test]
    public void ShouldSkipToNextActiveItem()
    {
        _checklist.SelectedItem = _checklist.Children[0];
        
        _checklist.Skip();
        
        Assert.That(_checklist.SelectedItem, Is.EqualTo(_checklist.Children[1]));
        
        _checklist.Skip();
        // should rewind
        Assert.That(_checklist.SelectedItem, Is.EqualTo(_checklist.Children[0]));
    }
    
    
}