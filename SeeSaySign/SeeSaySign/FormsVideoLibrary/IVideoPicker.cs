using System.Threading.Tasks;

namespace SeeSaySign.FormsVideoLibrary
{
    public interface IVideoPicker
    {
        Task<string> GetVideoFileAsync();
    }
}
