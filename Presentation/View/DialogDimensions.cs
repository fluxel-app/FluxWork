namespace FluxWork.Presentation.View
{
  public class DialogDimensions
  {
    public DialogDimensions(string key)
    {
      this.Width = 800;
      this.Height = 400;
      this.MinWidth = 800;
      this.MinHeight = 400;
      this.MaxWidth = 0;
      this.MaxHeight = 0;
      this.X = 0;
      this.Y = 0;
      this.Fullscreen = true;
    }

    public int Width { get; set; }

    public int Height { get; set; }

    public int MinWidth { get; set; }

    public int MinHeight { get; set; }

    public int MaxWidth { get; set; }

    public int MaxHeight { get; set; }

    public int X { get; set; }

    public int Y { get; set; }

    public bool Fullscreen { get; set; }

    public void SetHeights(int height)
    {
      this.MaxHeight = height;
      this.MinHeight = height;
      this.Height = height;
    }

    public void SetWidths(int width)
    {
      this.MaxWidth = width;
      this.MinWidth = width;
      this.Width = width;
    }
  }
}
