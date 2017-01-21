using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Video_Catalog
{
    public class mainMenu
    {
        /// GLOBAL VARS
        public string basePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Videos";
        public string basePathD = @"D:\Users\Sir Daniel\Videos";
        public string networkPath = @"Z:\Main (A)\Shared Videos"; /// Z:\Main (A)\Shared Videos\Movies
        public string tele = " Television ";
        public string movi = @"   Movies   ";
        public string space = @"                                                                                  ";
        videoPackage moviesByGenre = new videoPackage();
        public bool networkAvail = false;
        

        /// Util Init
        fileRetrievalUtil _fru = new fileRetrievalUtil();
        videoPlayerUtil _vpu = new videoPlayerUtil();
        printUtil _pu = new printUtil();

        public void initMenu()
        {
            if (Directory.Exists(networkPath))
            {
                networkAvail = true;
            }
            menu();
        }
        public void menu()
        {
            List<FileInfo> videos = new List<FileInfo>();
            _pu.write(_pu.br + "Please select one of the folowing options.", _pu.cyan);
            _pu.write(_pu.br + "Local Videos", _pu.cyan);
            _pu.write(_pu.br + "1) ", _pu.drkcyan);
            _pu.write("Movies.", _pu.wht);
            _pu.write(_pu.br + "2) ", _pu.drkcyan);
            _pu.write("Television.", _pu.wht);
            if (networkAvail)
            {
                _pu.write(_pu.br + "Network Videos", _pu.cyan);
                _pu.write(_pu.br + "3) ", _pu.drkcyan);
                _pu.write("Movies.", _pu.wht);
                _pu.write(_pu.br + "4) ", _pu.drkcyan);
                _pu.write("Television", _pu.wht);
                _pu.write(_pu.br + "5) ", _pu.drkcyan);
                _pu.write("Stand-up Comedy.", _pu.wht);
            }
            _pu.write(_pu.br + "X) ", _pu.drkcyan);
            _pu.write("Exit Application.", _pu.wht);
            ConsoleKeyInfo _selection = _pu.rk(_pu.br + "Selection: ", _pu.gray, _pu.cyan);
            switch (_selection.KeyChar)
            {
                case '1':
                    videos.AddRange(_fru.get(basePath + @"\Movies"));
                    videos.AddRange(_fru.get(basePathD + @"\Movies"));
                    videos = videos.OrderBy(x => x.FullName).ToList();
                    FileInfo movie = displayTitles(videos, movi);
                    _vpu.play(movie);
                    _pu.resetConsole();
                    menu();
                    break;
                case '2':
                    _fru.getVideoPackage(basePath + @"\Television");
                    _fru.getVideoPackage(basePathD + @"\Television");
                    videos.AddRange(_fru.get(basePath + @"\Television"));
                    videos.AddRange(_fru.get(basePathD + @"\Television"));
                    videos = videos.OrderBy(x => x.FullName).ToList();
                    FileInfo episode = displayTitles(videos, tele);
                    _vpu.play(episode);
                    _pu.resetConsole();
                    menu();
                    break;
                case '3':
                    if (networkAvail)
                    {
                        string Movie = _fru.getVideoPackage(networkPath + @"\Movies");
                        if (Movie != null)
                        {
                            FileInfo _movie = new FileInfo(Movie);
                            _vpu.play(_movie);
                        }
                    }
                    _pu.resetConsole();
                    menu();
                    break;
                case '4':
                    if (networkAvail)
                    {
                        string _episode = _fru.getVideoPackage(networkPath + @"\Television");
                        if (_episode != null)
                        {
                            FileInfo _Episode = new FileInfo(_episode);
                            _vpu.play(_Episode);
                        }
                    }
                    _pu.resetConsole();
                    menu();
                    break;
                case '5':
                    if (networkAvail)
                    {

                    }
                    _pu.resetConsole();
                    menu();
                    break;
                case 'X':
                case 'x':
                    Environment.Exit(0);
                    break;
                default:
                    _pu.resetConsole();
                    menu();
                    break;
            }
        }
        public FileInfo displayMovieByGenre(videoPackage _genre)
        {
            FileInfo _video = displayTitles(_genre.videos, movi);
            return _video;
        }
        public FileInfo displayTitles(List<FileInfo> videos, string videoType)
        {
            _pu.topBar(videoType);
            int count = 1;
            foreach (FileInfo video in videos)
            {
                if (video.Extension == ".avi" || video.Extension == ".mkv" || video.Extension == ".mp4")
                {
                    string videoName = "";
                    string _count = "\n\r| " + count + ")" + space;
                    _count = _count.Substring(0, 10);
                    _pu.write(_count + "| ", _pu.wht);
                    if (video.Name.ToCharArray().Count() > 75)
                    {
                        videoName = video.Name.Substring(0, 72) + "...";
                    }
                    else
                    {
                        videoName = (video.Name + space).Substring(0, 75);
                    }
                    _pu.write(videoName, _pu.cyan);
                    _pu.write(@"| Length         |", _pu.wht);
                    count += 1;
                }
            }
            FileInfo _video = getSelection(videos);
            return _video;
        }
        public FileInfo getSelection(List<FileInfo> videos)
        {
            int select;
            string _select;
            _select = _pu.rl(_pu.br + "Select index #: ", _pu.wht, _pu.cyan);
            bool result = int.TryParse(_select, out select);
            if (result)
            {
                if(select > 0 && select <= videos.Count())
                {
                    FileInfo thisVideo = videos[select - 1];
                    if(thisVideo != null)
                    {
                        return thisVideo;
                    }
                    else
                    {
                        _pu.write(_pu.br + _select + " is not a valid selection.", _pu.gray);
                        return getSelection(videos);
                    }
                }
                else
                {
                    _pu.write(_pu.br + _select + " is not a valid selection.", _pu.gray);
                    return getSelection(videos);
                }
            }
            else
            {
                _pu.write(_pu.br + _select + " is not a valid selection.", _pu.gray);
                return getSelection(videos);
            }
        }
    }
}
