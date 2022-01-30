namespace Chklstr.UI.Core.Utils;

public class HierarchyLevel
{
    public int[] Parts { get; private set; }

    public HierarchyLevel() : this(1, 1) {
        
    }

    public HierarchyLevel(int levels) : this(levels, 1) {
        
    }


    public HierarchyLevel(int levels, int value) {
        Parts = new int[levels];
        for (int i = 0 ; i < levels; i++) {
            Parts[i] = value;
        }
    }

    public HierarchyLevel Next() {
        var newHierarchy = new HierarchyLevel(this.Parts.Length);
        Array.Copy(this.Parts, 0, newHierarchy.Parts, 0, this.Parts.Length);
        newHierarchy.Parts[newHierarchy.Parts.Length - 1]++;
        return newHierarchy;
    }

    public HierarchyLevel Prefix(HierarchyLevel prefixLevel) {
        var newHierarchy = new HierarchyLevel(prefixLevel.Parts.Length + this.Parts.Length);
        Array.Copy(prefixLevel.Parts, 0, newHierarchy.Parts, 0, prefixLevel.Parts.Length);
        Array.Copy(this.Parts, 0, newHierarchy.Parts, prefixLevel.Parts.Length, this.Parts.Length);
        return newHierarchy;
    }

    public HierarchyLevel Indent() {
        var newHierarchy = new HierarchyLevel(this.Parts.Length + 1);
        Array.Copy(this.Parts, 0, newHierarchy.Parts, 0, this.Parts.Length);

        return newHierarchy;
    }

    public HierarchyLevel Unindent() {
        if (this.Parts.Length == 1) return this;
        var newHierarchy = new HierarchyLevel(this.Parts.Length - 1);
        if (this.Parts.Length - 1 >= 0) Array.Copy(this.Parts, 0, newHierarchy.Parts, 0, this.Parts.Length - 1);

        return newHierarchy;
    }

    public override string ToString() {
        return String.Join(".", this.Parts.Select(i => i.ToString()));
    }

}