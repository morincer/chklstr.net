using Chklstr.Core.Model;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Chklstr.Core.Tests;

public class ChecklistTest
{
    private Checklist checklist;

    [SetUp]
    public void SetUp() {
        this.checklist = new Checklist("Test", null);
        this.checklist.AddSingleItem("Test check 1", "CHECK");
        this.checklist.AddSingleItem("Test check 2", "CHECK");
    }

    [Test]
    public void ShouldBeMarkedCompleteWhenAllItemsAreChecked() {
        Assert.False(this.checklist.IsComplete());
        this.checklist.Items[0].Checked = true;
        Assert.False(this.checklist.IsComplete());
        this.checklist.Items[1].Checked = true;
        Assert.True(this.checklist.IsComplete());
    }

    [Test]
    public void ShouldBeMarkedCompleteWithRespectToContext() {
        var item1 = this.checklist.Items[0];
        var item2 = this.checklist.Items[1];
        item2.Contexts.Add("Ctx");

        Assert.False(this.checklist.IsComplete()); // nothing checked

        // item 1 is context free, item 2 is context aware - should be marked complete in default
        // context and incomplete in Ctx context
        item1.Checked = true;
        Assert.True(this.checklist.IsComplete());
        Assert.False(this.checklist.IsComplete("Ctx"));

        item2.Checked = true;
        Assert.True(this.checklist.IsComplete("Ctx"));
    }
}