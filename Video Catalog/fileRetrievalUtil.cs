using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Video_Catalog
{
    public class fileRetrievalUtil
    {
        /// INIT GLOBALS
        public string space = @"                                                                                                    ";
        printUtil _pu = new printUtil();
        public List<string> directories = new List<string>();

        public void multiplePaths(List<string> paths)
        {
            List<string> series = new List<string>();
            foreach(string path in paths)
            {
                series.AddRange(getSubfolders(path));
            }
            series = series.OrderBy(x => x.Split('\\').Last()).ToList();
            List<pagedData> seriesPages = createPages(series, false);
            string seriesPath = displayPages(seriesPages, 0);
            List<string> seasons = getSubfolders(seriesPath);
            if(seasons.Count() > 0)
            {
                List<pagedData> seasonPages = createPages(seasons, false);
                string selectedSeason = displayPages(seasonPages, 0);
            }
            List<string> episodes = getVideoPaths(seriesPath);
        }
        public string getVideoPackage(string directory)
        {
            videoPackage videoList = new videoPackage();
            string[] pathType = directory.Split('\\');
            if (pathType.First() == "Z:")
            {
                if (pathType.Last() == "Movies")
                {
                    List<string> movies = new List<string>();
                    directories = new List<string>();
                    List<string> genres = getSubfolders(directory);
                    List<pagedData> genreList = createPages(genres, false);
                    string selectedGenre = displayPages(genreList, 0);
                    if (selectedGenre == null) { return null; }
                    videoList.genreName = selectedGenre.Split('\\').Last();
                    string[] genreSubFolders = Directory.GetDirectories(selectedGenre);
                    foreach (string folder in genreSubFolders)
                    {
                        movies.AddRange(getVideoPaths(folder));
                    }
                    movies = movies.OrderBy(x => x.Split('\\').Last()).ToList();
                    List<pagedData> movieList = createPages(movies, true);
                    string selectedMovie = displayPages(movieList, 0);
                    return selectedMovie;
                }
                else if (pathType.Last() == "Television")
                {
                    string selectSeason = null;
                    string episode = "";
                    List<string> episodes = new List<string>();
                    List<string> series = getSeries(directory);
                    List<pagedData> pagedVideoPackage = createPages(series, false);
                    string selectSeries = displayPages(pagedVideoPackage, 0);
                    if (selectSeries == null) { return null; }
                    List<string> _seasons = getSeasons(selectSeries);
                    if (_seasons.Count() > 0)
                    {
                        List<pagedData> seasonPack = createPages(_seasons, false);
                        selectSeason = displayPages(seasonPack, 0);
                        if (selectSeason == null) { return null; }
                    }
                    if (selectSeason != null)
                    {
                        episodes = getVideoPaths(selectSeason);
                        List<pagedData> _episodes = createPages(episodes, true);
                        episode = displayPages(_episodes, 0);
                        if (episode == null) { return null; }
                    }
                    else
                    {
                        episodes = getVideoPaths(selectSeries);
                        List<pagedData> _episodes = createPages(episodes, true);
                        episode = displayPages(_episodes, 0);
                    }
                    return episode;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public List<string> getVideoPaths(string directory)
        {
            string[] tempVideoArray = Directory.GetFiles(directory);
            List<string> videos = new List<string>();
            foreach (string episode in tempVideoArray)
            {
                videos.Add(episode);
            }
            return videos;
        }
        public List<string> getSeasons(string seriesDirectory)
        {
            string[] _seasons = Directory.GetDirectories(seriesDirectory);
            List<string> seasonList = new List<string>();
            foreach (string _season in _seasons)
            {
                seasonList.Add(_season);
            }
            return seasonList;
        }
        public string displayPages(List<pagedData> pages, int currentPage)
        {
            int count = 0;
            _pu.resetConsole();
            _pu.topBarBlank();
            foreach (string item in pages[currentPage].dataList)
            {
                string _count = String.Format(count + ")        ").Substring(0, 6);
                string name = String.Format("| " + item.Split('\\').Last() + space).Substring(0, 94) + '|';
                _pu.write(_pu.br + "| ", _pu.wht);
                _pu.write(_count, _pu.drkcyan);
                _pu.write(name, _pu.wht);
                count += 1;
            }
            int prevPage = 0;
            int nextPage = 0;
            if (currentPage == 0) { prevPage = 0; }
            else { prevPage = currentPage - 1; }
            if (currentPage == pages.Count() - 1) { nextPage = currentPage; }
            else { nextPage = currentPage + 1; }
            _pu.pagedBottomBar(currentPage, prevPage, nextPage);
            _pu.write(_pu.br + "Left arrow ", _pu.cyan);
            _pu.write("for previous page, ", _pu.wht);
            _pu.write("Right arrow ", _pu.cyan);
            _pu.write("for next page, ", _pu.wht);
            _pu.write("X ", _pu.cyan);
            _pu.write("to abort.", _pu.wht);
            ConsoleKeyInfo key = _pu.rk("Or, select by index number: ", _pu.wht, _pu.cyan);
            if (key.Key == ConsoleKey.LeftArrow)
            {
                if (currentPage == 0)
                {
                    return displayPages(pages, 0);
                }
                else
                {
                    return displayPages(pages, currentPage - 1);
                }
            }
            else if (key.Key == ConsoleKey.RightArrow)
            {
                if (currentPage == pages.Count() - 1)
                {
                    return displayPages(pages, currentPage);
                }
                else
                {
                    return displayPages(pages, currentPage + 1);
                }
            }
            else
            {
                int _itemSelect;
                bool result = int.TryParse(key.KeyChar.ToString(), out _itemSelect);
                if (result)
                {
                    if (_itemSelect >= 0 && _itemSelect <= pages[currentPage].dataList.Count() - 1)
                    {
                        return pages[currentPage].dataList[_itemSelect];
                    }
                    else
                    {
                        _pu.write(_pu.br + _itemSelect + " is not a valid selection.", _pu.wht);
                        return displayPages(pages, currentPage);
                    }
                }
                else
                {
                    if (key.KeyChar == 'x' || key.KeyChar == 'X')
                    {
                        return null;
                    }
                    _pu.write(_pu.br + _itemSelect + " is not a valid selection.", _pu.wht);
                    return displayPages(pages, currentPage);
                }
            }
        }
        public List<pagedData> createPages(List<string> _data, bool video)
        {
            int count = 0;
            int pageCount = 0;
            _data = _data.OrderBy(x => x.Split('\\').Last()).ToList();
            List<pagedData> dataPages = new List<pagedData>();
            pagedData dataPage = new pagedData();
            dataPage.dataList = new List<string>();
            dataPage.pageNumber = pageCount;
            foreach (string _series in _data)
            {
                if (video == true)
                {
                    if (_series.Split('.').Last() == "mkv" || _series.Split('.').Last() == "mp4" || _series.Split('.').Last() == "avi")
                    {
                        count += 1;
                        dataPage.dataList.Add(_series);
                    }
                }
                else
                {
                    count += 1;
                    dataPage.dataList.Add(_series);
                }
                if (count == 10 || _data.IndexOf(_series) == _data.Count() - 1)
                {
                    count = 0;
                    pageCount += 1;
                    dataPages.Add(dataPage);
                    dataPage = new pagedData();
                    dataPage.dataList = new List<string>();
                    dataPage.pageNumber = pageCount;
                }
            }
            return dataPages;
        }
        public List<string> getSeries(string directory)
        {
            string[] seriesSubFolder = Directory.GetDirectories(directory);
            string[] seriesPack = { };
            List<string> seriesList = new List<string>();
            foreach (string subFolder in seriesSubFolder)
            {
                seriesPack = Directory.GetDirectories(subFolder);
                foreach (string series in seriesPack)
                {
                    seriesList.Add(series);
                }
            }
            return seriesList;
        }
        public List<string> getSubfolders(string directory)
        {
            string[] genresRAW = Directory.GetDirectories(directory).OrderBy(x => x.Split('\\').Last()).ToArray();
            List<string> genres = new List<string>();
            foreach (string genre in genresRAW)
            {
                genres.Add(genre);
            }
            return genres;
        }
        public string getGenre(string directory)
        {
            int _selection = 0;
            string[] genresRAW = Directory.GetDirectories(directory).OrderBy(x => x.Split('\\').Last()).ToArray();
            int count = 1;
            foreach (string genreRAW in genresRAW)
            {
                string _genre = genreRAW.Split('\\').Last();
                _pu.write(_pu.br + " " + count + ") ", _pu.cyan);
                _pu.write(_genre, _pu.wht);
                count += 1;
            }
            string selection = _pu.rl(" Select Genre: ", _pu.gray, _pu.cyan);
            bool result = int.TryParse(selection, out _selection);
            if (result)
            {
                if (_selection - 1 >= 0 && _selection <= genresRAW.Count())
                {
                    string genreChoice = genresRAW[_selection - 1];
                    if (genreChoice != null)
                    {
                        return genreChoice;
                    }
                    else
                    {
                        _pu.write(_pu.br + " There is no genre at index " + selection + ".", _pu.gray);
                        _pu.rest(2000);
                        _pu.resetConsole();
                        return getGenre(directory);
                    }
                }
                else
                {
                    _pu.write(_pu.br + " " + selection + " is not a valid index selction.", _pu.gray);
                    _pu.rest(2000);
                    _pu.resetConsole();
                    return getGenre(directory);
                }
            }
            else
            {
                _pu.write(_pu.br + " There is no genre at index " + selection + ".", _pu.gray);
                _pu.rest(2000);
                _pu.resetConsole();
                return getGenre(directory);
            }
        }
        public List<FileInfo> get(string directory)
        {
            List<FileInfo> videos = new List<FileInfo>();
            List<string> addtnlDirec = new List<string>();
            directories = new List<string>();
            addtnlDirec = direcDig(directory);
            string[] videosRAW = { };
            foreach (string d in addtnlDirec)
            {
                if (Directory.Exists(d))
                {
                    videosRAW = Directory.GetFiles(d);
                }
                if (videosRAW.Count() > 0)
                {
                    foreach (string videoRAW in videosRAW)
                    {
                        if (videoRAW != null)
                        {
                            FileInfo _file = new FileInfo(videoRAW);
                            videos.Add(_file);
                        }
                    }
                }
            }
            return videos;
        }
        public List<string> direcDig(string thisDirect)
        {
            directories.Add(thisDirect);
            string[] newDirects = Directory.GetDirectories(thisDirect);
            if (newDirects != null && newDirects.Count() > 0)
            {
                foreach (string newDirect in newDirects) { direcDig(newDirect); }
            }
            else { }
            return directories;
        }
        public List<FileInfo> getVideoFiles(List<string> videoPaths)
        {
            List<FileInfo> videoList = new List<FileInfo>();
            string[] tempVideoArray = { };
            foreach (string path in videoPaths)
            {
                tempVideoArray = Directory.GetFiles(path);
                foreach (string movie in tempVideoArray)
                {
                    FileInfo thisMovie = new FileInfo(movie);
                    videoList.Add(thisMovie);
                }

            }
            videoList = videoList.OrderBy(x => x.Name).ToList();
            return videoList;
        }
    }
}
