﻿using System;
using System.Threading.Tasks;
using Chklstr.Core.Model;
using Chklstr.UI.Core.ViewModels;
using MvvmCross.Tests;
using NUnit.Framework;

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
        
        _checklist = new ChecklistViewModel();
        _checklist.Prepare(cl);
        await _checklist.Initialize();
    }

    [Test]
    public void ShouldSetCounters()
    {
        Assert.That(_checklist.CheckableItemsCount, Is.EqualTo(1));
        Assert.That(_checklist.CheckedItemsCount, Is.EqualTo(0));
        Assert.That(_checklist.IsEnabled, Is.True);
    }

    [Test]
    public void ShouldUpdateCountersWithRespectToContexts()
    {
        _checklist.Item.Items[0].Contexts.Add("ctx");
        _checklist.UpdateCounters();
        
        Assert.That(_checklist.CheckableItemsCount, Is.EqualTo(0));
        Assert.That(_checklist.IsEnabled, Is.False);

        _checklist.Contexts = new[] {"ctx"};

        Assert.That(_checklist.CheckableItemsCount, Is.EqualTo(1));
        Assert.That(_checklist.IsEnabled, Is.True);
    }

    [Test]
    public void ShouldCreateChildrenWithListNumbers()
    {
        Assert.That(_checklist.Children, Is.Not.Empty);
        Assert.That(_checklist.Children[0].ListNumber, Is.EqualTo("1"));

    }
    
}