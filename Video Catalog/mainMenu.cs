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
            if (Directory.Exists(networkPath))
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
                    string[] localDirects = { basePath + @"\Movies", basePathD + @"\Movies" };
                    string selectedMovie = _fru.getVideoPackage(localDirects);
                    if(selectedMovie != null)
                    {
                        FileInfo _selectedFile = new FileInfo(selectedMovie);
                        _vpu.play(_selectedFile);
                    }
                    else
                    {
                        _pu.write(_pu.br + "Aborted explorer. Returning to main menu.", _pu.red);
                        _pu.rest(1500);
                    }
                    _pu.resetConsole();
                    menu();
                    break;
                case '2':
                    string[] _localDirects = { basePath + @"\Television", basePathD + @"\Television" };
                    string selectedEpisode = _fru.getVideoPackage(_localDirects);
                    if(selectedEpisode != null)
                    {
                        FileInfo episode = new FileInfo(selectedEpisode);
                        _vpu.play(episode);

                    }
                    else
                    {
                        _pu.write(_pu.br + "Aborted explorer. Returning to main menu.", _pu.red);
                        _pu.rest(1500);
                    }
                    _pu.resetConsole();
                    menu();
                    break;
                case '3':
                    if (networkAvail)
                    {
                        string[] videoPaths = { networkPath + @"\Movies" };
                        string Movie = _fru.getVideoPackage(videoPaths);
                        if (Movie != null)
                        {
                            FileInfo _movie = new FileInfo(Movie);
                            _vpu.play(_movie);
                        }
                        else
                        {
                            _pu.write(_pu.br + "Aborted explorer. Returning to main menu.", _pu.red);
                            _pu.rest(1500);
                        }
                    }
                    _pu.resetConsole();
                    menu();
                    break;
                case '4':
                    if (networkAvail)
                    {
                        string[] videoPaths = { networkPath + @"\Television" };
                        string _episode = _fru.getVideoPackage(videoPaths);
                        if (_episode != null)
                        {
                            FileInfo _Episode = new FileInfo(_episode);
                            _vpu.play(_Episode);
                        }
                        else
                        {
                            _pu.write(_pu.br + "Aborted explorer. Returning to main menu.", _pu.red);
                            _pu.rest(1500);
                        }
                    }
                    _pu.resetConsole();
                    menu();
                    break;
                case '5':
                    if (networkAvail)
                    {
                        string[] videoPaths = { networkPath + @"\Stand up Comedy" };
                        string _episode = _fru.getVideoPackage(videoPaths);
                        if (_episode != null)
                        {
                            FileInfo _Episode = new FileInfo(_episode);
                            _vpu.play(_Episode);
                        }
                        else
                        {
                            _pu.write(_pu.br + "Aborted explorer. Returning to main menu.", _pu.red);
                            _pu.rest(1500);
                        }
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
    }
}
