public class StackedImage
{
    public int _topIdx;
    public int _neckIdx;
    public int _contentTopIdx;
    public int _contentBottomIdx;
    public StackedImage(int topIdx, int neckIdx, int contentTopIdx, int contentBottomIdx)
    {
        _topIdx = topIdx;
        _neckIdx = neckIdx;
        _contentTopIdx = contentTopIdx;
        _contentBottomIdx = contentBottomIdx;
    }
}