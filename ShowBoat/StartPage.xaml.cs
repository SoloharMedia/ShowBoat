using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ShowBoat
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StartPage : Page
    {
        int resultCount = 10;

        List<bool> movieAdded = new List<bool>();
        List<string> movieTitles = new List<string>();
        List<string> movieYears = new List<string>();
        List<string> movieOverviews = new List<string>();
        List<string> fullMovieOverviews = new List<string>();
        List<string> movieVotes = new List<string>();
        List<string> moviePosters = new List<string>();
        List<string> movieIDs = new List<string>();
        List<string> movieGenres = new List<string>();

        //Timer For Automatic FlipView For Theatre Movies
        DispatcherTimer flipViewTimer = new DispatcherTimer();

        public StartPage()// Start of StartPage
        {
            this.InitializeComponent();
            sbStartPageLogo.Begin();
            GrabMoviesFromTheatre();
            flipViewTimer.Interval = new TimeSpan(0, 0, 0, 0, 5000);
            flipViewTimer.Tick += ChangeMovie;
            flipViewTimer.Start();
        }
        //Automatic FlipView Changer
        private void ChangeMovie(object sender, object o)
        {
            //Gets The Number Of Items In FlipView
            var totalItems = fvTheatre.Items.Count;
            //Gets The New Items Index (Current Index Plus One)~ If Next Index Is Out Of Range, Moves Back To First
            var newItemIndex = (fvTheatre.SelectedIndex + 1) % totalItems;
            //Set Displayed FlipViewItem's Index
            fvTheatre.SelectedIndex = newItemIndex;
        }

        //Navigation Menu
        private void NavigateToNewPage(object sender, TappedRoutedEventArgs e)
        {
            Image clickedImage = sender as Image;
            string userClicked = clickedImage.Name;

            if (userClicked == "btnNavSearch")
            {
                Frame.Navigate(typeof(MainPage));
            }
            if (userClicked == "btnNavList")
            {
                Frame.Navigate(typeof(MyMovies));
            }
            if (userClicked == "btnNavTrivia")
            {
                Frame.Navigate(typeof(Trivia));
            }
        }

        //Grabs Movies From Thatre Using TMDB API
        private async void GrabMoviesFromTheatre()
        {
            movieAdded.Clear();
            movieTitles.Clear();
            movieYears.Clear();
            movieOverviews.Clear();
            fullMovieOverviews.Clear();
            movieVotes.Clear();
            moviePosters.Clear();
            movieIDs.Clear();
            movieGenres.Clear();

            var client = new HttpClient();
            string searchPart1 =
                "https://api.themoviedb.org/3/movie/now_playing?api_key=1f324761abbf02c4cdffa8505733a8c0&language=en-US&page=1";
            HttpResponseMessage data = await client.GetAsync(
                new Uri(searchPart1));

            var fullString = await data.Content.ReadAsStringAsync();

            // first get the result count out of the data
            int startPos = 0;
            int endPos = 0;
            if (!fullString.Contains("errors"))
            {
                int startingFrom = 0;
                string titleToAdd = "";
                // grab movie titles
                for (int i = 0; i < resultCount; i++)
                {
                    startPos = fullString.IndexOf("\"title\":", startingFrom);
                    startPos += 9;
                    endPos = fullString.IndexOf(",\"", startPos);
                    titleToAdd = fullString.Substring(
                        startPos, (endPos - startPos - 1));
                    if (titleToAdd == "")
                    {
                        movieAdded.Add(false);
                        movieTitles.Add("");
                    }
                    else
                    {
                        movieAdded.Add(true);
                        movieTitles.Add(titleToAdd);
                    }
                    startingFrom = endPos + 1;
                }

                startingFrom = 0;
                // grab the release year
                for (int i = 0; i < resultCount; i++)
                {
                    if (movieAdded[i] == false)
                    {
                        movieYears.Add("");
                    }
                    else
                    {
                        startPos = fullString.IndexOf("\"release_date\":",
                            startingFrom);
                        startPos += 16;
                        endPos = fullString.IndexOf(",\"", startPos);
                        movieYears.Add(fullString.Substring(startPos, 10));
                        startingFrom = endPos + 1;
                    }
                }

                startingFrom = 0;

                // grab the Genre
                for (int i = 0; i < resultCount; i++)
                {
                    if (movieAdded[i] == false)
                    {
                        movieYears.Add("");
                    }
                    else
                    {
                        startPos = fullString.IndexOf("\"genre_ids\":",
                            startingFrom);
                        startPos += 13;
                        endPos = fullString.IndexOf(",", startPos);
                        string tempGenre = fullString.Substring(startPos, (endPos - startPos));
                        //Makes it grab the first Genre If given Multiple
                        if (tempGenre.Contains("]"))//<-- If multiple Genres, Will Delete excess "]" character
                        {
                            string editedGenre = tempGenre.Replace("]", "");

                            //Simply Converts Genre ID # To Written String of Genre
                            switch (editedGenre.ToString())
                            {
                                case "28":
                                    {
                                        movieGenres.Add("Action");
                                        break;
                                    };
                                case "12":
                                    {
                                        movieGenres.Add("Adventure");
                                        break;
                                    };
                                case "16":
                                    {
                                        movieGenres.Add("Animation");
                                        break;
                                    };
                                case "35":
                                    {
                                        movieGenres.Add("Comedy");
                                        break;
                                    };
                                case "80":
                                    {
                                        movieGenres.Add("Crime");
                                        break;
                                    };
                                case "99":
                                    {
                                        movieGenres.Add("Documentary");
                                        break;
                                    };
                                case "18":
                                    {
                                        movieGenres.Add("Drama");
                                        break;
                                    };
                                case "10751":
                                    {
                                        movieGenres.Add("Family");
                                        break;
                                    };
                                case "14":
                                    {
                                        movieGenres.Add("Fantasy");
                                        break;
                                    };
                                case "36":
                                    {
                                        movieGenres.Add("History");
                                        break;
                                    };
                                case "27":
                                    {
                                        movieGenres.Add("Horror");
                                        break;
                                    };
                                case "10402":
                                    {
                                        movieGenres.Add("Music");
                                        break;
                                    };
                                case "9648":
                                    {
                                        movieGenres.Add("Mystery");
                                        break;
                                    };
                                case "10749":
                                    {
                                        movieGenres.Add("Romance");
                                        break;
                                    };
                                case "878":
                                    {
                                        movieGenres.Add("Science-Fiction");
                                        break;
                                    };
                                case "10770":
                                    {
                                        movieGenres.Add("TV-Movie");
                                        break;
                                    };
                                case "53":
                                    {
                                        movieGenres.Add("Thriller");
                                        break;
                                    };
                                case "10752":
                                    {
                                        movieGenres.Add("War");
                                        break;
                                    };
                                case "37":
                                    {
                                        movieGenres.Add("Western");
                                        break;
                                    };
                                default:
                                    {
                                        movieGenres.Add("Unknown Genre");
                                        break;
                                    };
                            }
                        }
                        else
                        {
                            switch (tempGenre.ToString())
                            {
                                case "28":
                                    {
                                        movieGenres.Add("Action");
                                        break;
                                    };
                                case "12":
                                    {
                                        movieGenres.Add("Adventure");
                                        break;
                                    };
                                case "16":
                                    {
                                        movieGenres.Add("Animation");
                                        break;
                                    };
                                case "35":
                                    {
                                        movieGenres.Add("Comedy");
                                        break;
                                    };
                                case "80":
                                    {
                                        movieGenres.Add("Crime");
                                        break;
                                    };
                                case "99":
                                    {
                                        movieGenres.Add("Documentary");
                                        break;
                                    };
                                case "18":
                                    {
                                        movieGenres.Add("Drama");
                                        break;
                                    };
                                case "10751":
                                    {
                                        movieGenres.Add("Family");
                                        break;
                                    };
                                case "14":
                                    {
                                        movieGenres.Add("Fantasy");
                                        break;
                                    };
                                case "36":
                                    {
                                        movieGenres.Add("History");
                                        break;
                                    };
                                case "27":
                                    {
                                        movieGenres.Add("Horror");
                                        break;
                                    };
                                case "10402":
                                    {
                                        movieGenres.Add("Music");
                                        break;
                                    };
                                case "9648":
                                    {
                                        movieGenres.Add("Mystery");
                                        break;
                                    };
                                case "10749":
                                    {
                                        movieGenres.Add("Romance");
                                        break;
                                    };
                                case "878":
                                    {
                                        movieGenres.Add("Science-Fiction");
                                        break;
                                    };
                                case "10770":
                                    {
                                        movieGenres.Add("TV-Movie");
                                        break;
                                    };
                                case "53":
                                    {
                                        movieGenres.Add("Thriller");
                                        break;
                                    };
                                case "10752":
                                    {
                                        movieGenres.Add("War");
                                        break;
                                    };
                                case "37":
                                    {
                                        movieGenres.Add("Western");
                                        break;
                                    };
                                default:
                                    {
                                        movieGenres.Add("Unknown Genre");
                                        break;
                                    };
                            }//end switch
                        }//end else
                        startingFrom = endPos + 1;
                    }
                }
                startingFrom = 0;
                // grab the movie overview
                for (int i = 0; i < resultCount; i++)
                {
                    if (movieAdded[i] == false)
                    {
                        movieOverviews.Add("");
                    }
                    else
                    {
                        startPos = fullString.IndexOf("\"overview\":", startingFrom);
                        startPos += 12;
                        endPos = fullString.IndexOf(",\"", startPos);
                        string theOverview = fullString.Substring(
                            startPos, (endPos - startPos - 1));
                        fullMovieOverviews.Add(theOverview);
                        if (theOverview.Length > 300)
                        {
                            string edit = theOverview.Substring(0, 300) + ".....";
                            movieOverviews.Add(edit);
                        }
                        else
                        {
                            movieOverviews.Add(theOverview);
                        }
                        startingFrom = endPos + 1;
                    }
                }

                startingFrom = 0;

                // grab the movie ID
                for (int i = 0; i < resultCount; i++)
                {
                    if (movieAdded[i] == false)
                    {
                        movieIDs.Add("");
                    }
                    else
                    {
                        startPos = fullString.IndexOf("\"id\":", startingFrom);
                        startPos += 5;
                        endPos = fullString.IndexOf(",\"", startPos);
                        movieIDs.Add(fullString.Substring(
                            startPos, (endPos - startPos)));
                        startingFrom = endPos + 1;



                    }
                }
                startingFrom = 0;
                // grab the average vote
                for (int i = 0; i < resultCount; i++)
                {
                    if (movieAdded[i] == false)
                    {
                        movieVotes.Add("");
                    }
                    else
                    {
                        //,"vote_count":0
                        startPos = fullString.IndexOf("\"vote_average\":", startingFrom);
                        startPos += 15;
                        endPos = fullString.IndexOf("}", startPos) + 1;
                        movieVotes.Add(fullString.Substring(
                            startPos, (endPos - startPos - 1)));
                        startingFrom = endPos + 1;
                    }
                }
                startingFrom = 0;
                //grabs posters of movies
                for (int i = 0; i < resultCount; i++)
                {
                    if (movieAdded[i] == false)
                    {
                        moviePosters.Add("");
                    }
                    else
                    {
                        startPos = fullString.IndexOf("\"poster_path\":", startingFrom);
                        startPos += 16;
                        endPos = fullString.IndexOf(",\"", startPos);
                        moviePosters.Add("http://image.tmdb.org/t/p/w150/" + fullString.Substring(
                            startPos, (endPos - startPos - 1)));
                        startingFrom = endPos + 1;
                    }
                }
            }
            DisplayMoviesInTheatre();
        }//end GetMoviesFromTheTheatre

        //Displays 10 Theatre Movies In The FlipView Items HardCoded
        private void DisplayMoviesInTheatre()
        {
            //Fills Theatre Results Posters
            imgTheatrePoster0.Source = new BitmapImage(new Uri(this.BaseUri, moviePosters[0]));
            imgTheatrePoster1.Source = new BitmapImage(new Uri(this.BaseUri, moviePosters[1]));
            imgTheatrePoster2.Source = new BitmapImage(new Uri(this.BaseUri, moviePosters[2]));
            imgTheatrePoster3.Source = new BitmapImage(new Uri(this.BaseUri, moviePosters[3]));
            imgTheatrePoster4.Source = new BitmapImage(new Uri(this.BaseUri, moviePosters[4]));
            imgTheatrePoster5.Source = new BitmapImage(new Uri(this.BaseUri, moviePosters[5]));
            imgTheatrePoster6.Source = new BitmapImage(new Uri(this.BaseUri, moviePosters[6]));
            imgTheatrePoster7.Source = new BitmapImage(new Uri(this.BaseUri, moviePosters[7]));
            imgTheatrePoster8.Source = new BitmapImage(new Uri(this.BaseUri, moviePosters[8]));
            imgTheatrePoster9.Source = new BitmapImage(new Uri(this.BaseUri, moviePosters[9]));

            //Fills Theatre Results Titles
            txtTheatreTitle0.Text = movieTitles[0];
            txtTheatreTitle1.Text = movieTitles[1];
            txtTheatreTitle2.Text = movieTitles[2];
            txtTheatreTitle3.Text = movieTitles[3];
            txtTheatreTitle4.Text = movieTitles[4];
            txtTheatreTitle5.Text = movieTitles[5];
            txtTheatreTitle6.Text = movieTitles[6];
            txtTheatreTitle7.Text = movieTitles[7];
            txtTheatreTitle8.Text = movieTitles[8];
            txtTheatreTitle9.Text = movieTitles[9];

            //Fills Theatre Results Years
            txtTheatreYear0.Text = movieYears[0];
            txtTheatreYear1.Text = movieYears[1];
            txtTheatreYear2.Text = movieYears[2];
            txtTheatreYear3.Text = movieYears[3];
            txtTheatreYear4.Text = movieYears[4];
            txtTheatreYear5.Text = movieYears[5];
            txtTheatreYear6.Text = movieYears[6];
            txtTheatreYear7.Text = movieYears[7];
            txtTheatreYear8.Text = movieYears[8];
            txtTheatreYear9.Text = movieYears[9];

            //Fills Theatre OverViews For Results
            txtTheatreOverview0.Text = movieOverviews[0];
            txtTheatreOverview1.Text = movieOverviews[1];
            txtTheatreOverview2.Text = movieOverviews[2];
            txtTheatreOverview3.Text = movieOverviews[3];
            txtTheatreOverview4.Text = movieOverviews[4];
            txtTheatreOverview5.Text = movieOverviews[5];
            txtTheatreOverview6.Text = movieOverviews[6];
            txtTheatreOverview7.Text = movieOverviews[7];
            txtTheatreOverview8.Text = movieOverviews[8];
            txtTheatreOverview9.Text = movieOverviews[9];
        }


        //Navigates To Adding Movie Part Once User Clicks (MainPage)
        private void DisplayTheatreMovie(object sender, TappedRoutedEventArgs e)
        {
            //Grabs the selected Index needed to display rest of Info
            Grid clickedGrid = sender as Grid;
            string selectedIndexChar = clickedGrid.Name.Substring((clickedGrid.Name.Length - 1), 1);
            int selIndex = Convert.ToInt32(selectedIndexChar);

            //Bundles String Into Package For Shipping
            string selectedMovie = movieTitles[selIndex] + "|" + movieYears[selIndex] + "|" +
                fullMovieOverviews[selIndex] + "|" + moviePosters[selIndex] + "|" + movieGenres[selIndex] +
                "|" + movieIDs[selIndex];

            Frame.Navigate(typeof(MainPage), selectedMovie);
        }
    }
}
