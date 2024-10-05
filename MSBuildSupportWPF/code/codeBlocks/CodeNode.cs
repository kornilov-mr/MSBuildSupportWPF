using System;
using System.Drawing;
using System.Security.Principal;
using System.Windows.Documents;
using System.Windows.Media;
using ColorMedia = System.Windows.Media.Color;
using Color = System.Drawing.Color;
using System.Windows;
using System.Windows.Controls;


namespace MSBuildSupport.code.codeBlocks;

public abstract class CodeNode
{
    //Code node before
    private CodeNode? parent;
    //Code node after
    private CodeNode? child;
    //Block tree this code node belongs to. Used to notify Block tree about changing
    public BlockTree BlockTreeAttached { get; set; }
    public CodeNode Parent
    {
        get => parent;
        set => parent = value ?? throw new ArgumentNullException(nameof(value));
    }

    public CodeNode Child
    {
        get => child;
        set => child = value ?? throw new ArgumentNullException(nameof(value));
    }
    //Text block, that will be shown on screen
    public Run CodeRun { get; set; } = new Run();
    //Content of this Node
    private string _codePart = string.Empty;
    public string CodePart { get
        {
            return _codePart;
        }
        set
        {
            _codePart = value;
            CodeRun.Text = value;
        }
    }
    //Color of hightlighting
    public Color HightLightColor { get; set; }
    //Start offset from start of text
    public int Position { get; set; }
    //Length of this Node
    public int Lenght { get; set; }

    protected CodeNode(int lenght, int position) : this(lenght, position, Color.Gray)
    { }
    protected CodeNode(int lenght, int position, string codePart) : this(lenght, position, Color.Gray, codePart)
    { }

    protected CodeNode(int lenght, int position, Color hightLightColor)
    {
        HightLightColor = hightLightColor;
        Position = position;
        Lenght = lenght;
        CodePart = new string(' ', lenght);

        CodeRun.Foreground = new SolidColorBrush(GetMediaColor());

    }
    protected CodeNode(int lenght, int position, Color hightLightColor, string codePart)
    {
        HightLightColor = hightLightColor;
        Position = position;
        Lenght = lenght;
        CodePart = codePart;

        CodeRun.Foreground = new SolidColorBrush(GetMediaColor());

    }
    //Revalidation only in this node, can lead to mismatch real offset on screen and offset in this code node
    protected void RevalidatePositionBasedOnParent()
    {
        Position = parent.Position + parent.Lenght;
    }
    //Recursion function on all code node before this, revalidation all offset before 
    public virtual void RevalidateAllPositionBefore()
    {
        parent.RevalidateAllPositionBefore();
        Position = parent.Position + parent.Lenght;
    }
    //Recursion function on all code node after this, revalidation all offset after 
    public virtual void RevalidateAllPositionAfter()
    {
        Position = parent.Position + parent.Lenght;
        if (child is null) return;
        child.RevalidateAllPositionAfter();

    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (!(obj is CodeNode)) return false;
        CodeNode other = (CodeNode)obj;
        if (other.Position != Position) return false;
        if (other.Lenght != Lenght) return false;
        return true;
    }
    //Function to convert Window.Drawing.Color to Window.Media.Color Because Window.Brush requires Media.Color
    public ColorMedia GetMediaColor()
    {
        return ColorMedia.FromRgb(HightLightColor.R, HightLightColor.G, HightLightColor.B);
    }
    //Function to Insert char inside code Node
    public virtual void InsertChar(char c, int position)
    {
        CodePart = _codePart.Insert(position, c.ToString());
        Lenght += 1;
        //Recalculates all position before
        RevalidateAllPositionAfter();
        //Notifies Block tree that node is changed
        notifyBlockTree();
    }
    //Function to insert string inside code Node

    public virtual void InsertString(string s, int position)
    {
        CodePart = _codePart.Insert(position, s);
        Lenght += s.Length;
        //Recalculates all position before
        RevalidateAllPositionAfter();
        //Notifies Block tree that node is changed
        notifyBlockTree();
    }
    //Function to delete char on sertain position inside code Node
    public virtual void DeleteChar(int position)
    {
        CodePart = _codePart.Remove(position-1, 1);
        Lenght -= 1;
        //Checks if code node is empty and deletes in from blockTree if so
        checkIfLengthIsZero();
        //Notifies Block tree that node is changed
        notifyBlockTree();
    }
    //Function to delete string from startGiven index to endGiven index inside code Node

    public virtual void DeleteString(int startGiven, int endGiven)
    {
        int start = startGiven;
        int end= endGiven;
        if (endGiven > Lenght)
        {
            end = Lenght;
        }
        if (startGiven < 0)
        {
            start = 0;
        }
        CodePart = _codePart.Remove(start, end-start);
        Lenght -= end - start;
        //Checks if code node is empty and deletes in from blockTree if so
        checkIfLengthIsZero();
        //Notifies Block tree that node is changed
        notifyBlockTree();

    }
    //Function which hightlight code node 
    public virtual void LightAsError(Exception e)
    {
        ColorMedia colorMedia = ColorMedia.FromRgb(255, 0, 0);
        CodeRun.Foreground = new SolidColorBrush(colorMedia);
        CodeRun.TextDecorations = TextDecorations.Underline;
        StartShowingPopup(e);
    }
    public void StartShowingPopup(Exception e)
    {
        CodeRun.MouseEnter += (sender, args) =>
        {
            BlockTreeAttached.ShowErrorPopup(e, Position);
        };
        CodeRun.MouseLeave += (sender, args) =>
        {
            BlockTreeAttached.HideErrorPopup();
        };
    }
    private void notifyBlockTree()
    {
        if(BlockTreeAttached != null)
        {
            BlockTreeAttached.NotifyAboutChangesInNode();
        }
    }
    public void checkIfLengthIsZero()
    {
        if (Lenght == 0)
        {
            BlockTreeAttached.ExcludeNodeFromTree(this);
        }
        else
        {
            RevalidateAllPositionAfter();
        }
    }
    public override string ToString()
    {
        string s = string.Empty;
        s += Lenght+" ";
        s += Position + " ";
        s += _codePart + " ";
        return s;
    }
}