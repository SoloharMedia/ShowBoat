using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using System.Net.Http;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using Windows.UI.Xaml.Shapes;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ShowBoat
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //Variable For Theatre Movies
        bool theatreMovie = false;

        //Variables For Search Results
        string movieItem = "";
        string omdbSearch = "";
        int resultCount = 0;
        string showBoatRating = "";
        string trueOrFalse = "";
        int selMovieID = 0;

        List<string> movies = new List<string>();
        List<bool> movieAdded = new List<bool>();
        List<string> movieTitles = new List<string>();
        List<string> movieYears = new List<string>();
        List<string> movieOverviews = new List<string>();
        List<string> fullMovieOverviews = new List<string>();
        List<string> movieVotes = new List<string>();
        List<string> moviePosters = new List<string>();
        List<string> movieIDs = new List<string>();
        List<string> movieCast = new List<string>();
        List<string> movieCastImage = new List<string>();
        List<string> movieGenres = new List<string>();
        List<string> movieCastChar = new List<string>();

        //List For Transporting Needed Data
        List<string> chosenMovieParts = new List<string>();

        //Variables For Trivia
        string triviaItem = "";
        List<string> movieTaglineParts = new List<string>();
        List<string> movieTaglines = new List<string>();

        public MainPage()//Starting Point Of Page
        {
            this.InitializeComponent();
        }

        //Displays Search Results(Creating Elements(views) As Needed)
        public void DisplayMovies()
        {
            chosenMovieParts.Clear();
            spMainMovieResults.Items.Clear();
            spMainMovieResults.Visibility = Visibility.Visible; 
            for (int i = 0; i < resultCount; i++)
            {
                //Creates StackPanel For Each Result
                StackPanel newSP = new StackPanel();
                newSP.Name = "spResult" + i;
                newSP.Orientation = Orientation.Horizontal;
                newSP.Height = 120;
                newSP.VerticalAlignment = VerticalAlignment.Top;
                newSP.HorizontalAlignment = HorizontalAlignment.Stretch;
                newSP.BorderThickness = new Thickness(1);
                newSP.BorderBrush = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.LightGray);
                //Adds The New StackPanel
                spMainMovieResults.Items.Add(newSP);

                //Creating Image For Poster
                Image newIMG = new Image();
                newIMG.Name = "moviePoster" + i;
                newIMG.Width = 80;
                newIMG.VerticalAlignment = VerticalAlignment.Stretch;
                newIMG.Margin = new Thickness(2);
                //Adds Img Placement In the New StackPanel
                newSP.Children.Add(newIMG);

                //Creates Second StackPanel For TextBlocks
                StackPanel secondStackPanel = new StackPanel();
                secondStackPanel.Name = "innerSP" + i;
                secondStackPanel.Width = 400;
                //Adds Second StackPanel to First StackPanel
                newSP.Children.Add(secondStackPanel);

                //Creates Title PlaceMent For Movie
                TextBlock tempMovieTitle = new TextBlock();
                tempMovieTitle.Name = "txtMovieTitleResult" + i;
                tempMovieTitle.FontSize = 13;
                tempMovieTitle.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.LightGray);
                tempMovieTitle.FontFamily = new FontFamily("Cambria");
                tempMovieTitle.Margin = new Thickness(0, 5, 0, 0);
                tempMovieTitle.TextAlignment = TextAlignment.Center;
                //Adds Title Placement To StackPanel
                secondStackPanel.Children.Add(tempMovieTitle);

                //Creates Year Placement For Movie Result
                TextBlock tempMovieYear = new TextBlock();
                tempMovieYear.Name = "txtMovieYearResult" + i;
                tempMovieYear.FontSize = 10;
                tempMovieYear.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.LightGray);
                tempMovieYear.FontFamily = new FontFamily("Cambria");
                tempMovieYear.TextAlignment = TextAlignment.Center;
                //Adds To StackPanel
                secondStackPanel.Children.Add(tempMovieYear);

                //Creates Horizontal Line
                Rectangle tempLine = new Rectangle();
                tempLine.Name = "theLine" + i;
                tempLine.Height = 1;
                tempLine.Fill = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.LightGray);
                tempLine.Margin = new Thickness(30, 10, 30, 0);
                //Adds it To StackPanel
                secondStackPanel.Children.Add(tempLine);

                //Creates Last But Not Least, the Description Of the Movie
                TextBlock tempMovieDesc = new TextBlock();
                tempMovieDesc.Name = "txtMovieDescResult" + i;
                tempMovieDesc.Width = 400;
                tempMovieDesc.Padding = new Thickness(7);
                tempMovieDesc.FontSize = 9;
                tempMovieDesc.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.LightGray);
                tempMovieDesc.TextWrapping = TextWrapping.Wrap;
                //Adds To StackPanel
                secondStackPanel.Children.Add(tempMovieDesc);

                //Places All Info For all Results
                if (moviePosters.Count >= 1)//<---Avoids Breaking down when people Backspace to Blank
                {
                    newIMG.Source = new BitmapImage(new Uri(this.BaseUri, moviePosters[i]));
                    tempMovieTitle.Text = movieTitles[i];
                    tempMovieYear.Text = movieYears[i];
                    tempMovieDesc.Text = movieOverviews[i];
                }
            }
        }
        //Searches TMDB API (As User Types)
        private async void GetData(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            //Clears for fresh list on every character Change
            movieIDs.Clear();
            movieAdded.Clear();
            movieTitles.Clear();
            movieYears.Clear();
            movieOverviews.Clear();
            movieVotes.Clear();
            moviePosters.Clear();
            movieCast.Clear();
            movieCastImage.Clear();
            fullMovieOverviews.Clear();
            movieGenres.Clear();
            movieTaglines.Clear();
            Canvas.SetZIndex(theMovieDisplay, 1);
            var client = new HttpClient();
            omdbSearch = txtSearch.Text;
            string searchPart1 =
                "https://api.themoviedb.org/3/search/movie?api_key=1f324761abbf02c4cdffa8505733a8c0&query=";

            HttpResponseMessage data = await client.GetAsync(
                new Uri(searchPart1 + omdbSearch));

            var fullString = await data.Content.ReadAsStringAsync();

            // first get the result count out of the data
            int startPos = 0;
            int endPos = 0;
            if (!fullString.Contains("errors"))
            {
                startPos = fullString.IndexOf("\"total_results\":") + 16;
                endPos = fullString.IndexOf(",\"total_pages\"");
                resultCount = Convert.ToInt32(
                    fullString.Substring(startPos, endPos - startPos));
                if (resultCount > 15)
                {
                    resultCount = 15; // eliminates page two
                }
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
                        movieYears.Add(fullString.Substring(startPos, 4));
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
            } // end if
            //Once info is Grabbed, Heads To DisplayMovies() To Show Info
            DisplayMovies();
        } // end GetData
        
        //Upon User Clicking Movie, Uses ID To Grab Cast & Cast Images
        public async void GetCast(string movieData)
        {
            int startPosition = 0;
            if(theatreMovie == false)
            {
                for (int i = 0; i < 7; i++)
                {
                    //Breaks Down String Into Parts (Need ID For Cast) ~~If Not a Theatre Movie
                    int endPosition = movieData.IndexOf("|", startPosition);
                    chosenMovieParts.Add(movieData.Substring(
                            startPosition, (endPosition - startPosition)));
                    startPosition = endPosition + 1;
                }
            }
            theatreMovie = true;
            //Uses ID To Get Top 3 Cast
            var client = new HttpClient();
            HttpResponseMessage data = await client.GetAsync(
                new Uri("http://api.themoviedb.org/3/movie/" + chosenMovieParts[0].ToString() + "/casts?api_key=1f324761abbf02c4cdffa8505733a8c0"));
            var fullString = await data.Content.ReadAsStringAsync();

            //Grabs The Top 3 Paid Cast
            int startingFrom = 0;
            for (int i = 0; i < 3; i++)
            {
                if (!fullString.Contains("\"name\":"))
                {
                    movieCast.Add("Unknown");
                }
                else
                {
                    int startPos = fullString.IndexOf("\"name\":", startingFrom);
                    startPos += 8;
                    int endPos = fullString.IndexOf(",\"", startPos);
                    movieCast.Add(fullString.Substring(
                        startPos, (endPos - startPos - 1)));
                    startingFrom = endPos + 1;
                }
            }
            startingFrom = 0;
            //Grabs The Top 3 Paid Cast Images
            for (int i = 0; i < movieCast.Count; i++)
            {
                if (fullString.Contains("profile_path\":\"/"))
                {
                    int startPos = fullString.IndexOf("\"profile_path\":", startingFrom);
                    startPos += 16;
                    int endPos = fullString.IndexOf("\"},", startPos);
                    movieCastImage.Add("http://image.tmdb.org/t/p/w150/" + fullString.Substring(
                        startPos, (endPos - startPos)));
                    startingFrom = endPos + 1;
                }
                else
                {
                    //Adds Placement Image If No Image was Found In Data
                    movieCastImage.Add("Assets/Images/tempImage.jpg");
                }
            }
            //Grabs Tagline For Further Use With Trivia
            GrabTagline(chosenMovieParts[0]);
            //Displays Specific Movie Chosen
            DisplayChosenMovie(selMovieID);
        }//End GetCast

        //Grabs TagLing For Trivia
        public async void GrabTagline(string movieID)
        {
            var client = new HttpClient();
            HttpResponseMessage data = await client.GetAsync(
                new Uri("http://api.themoviedb.org/3/movie/" + chosenMovieParts[0].ToString() + "?api_key=1f324761abbf02c4cdffa8505733a8c0"));
            var fullString = await data.Content.ReadAsStringAsync();
            
            int startPos = fullString.IndexOf("\"tagline\":", 0);
            startPos += 11;
            int endPos = fullString.IndexOf(",\"", startPos);
            movieTaglines.Add(fullString.Substring(
                startPos, (endPos - startPos - 1)));
        }

        //Gathers Data Of Movie Clicked From Search Results
        public void ShowResults(object sender, SelectionChangedEventArgs e)
        {
            spMainMovieResults.Visibility = Visibility.Collapsed;
            int selID = spMainMovieResults.SelectedIndex;

            //Throws All info for selected movie into a string Ready for Transport
            if(selID >= 0)
            {
                string editedOverview = fullMovieOverviews[selID].Replace(";", "");
                string movieData = movieIDs[selID] + "|" + movieTitles[selID] + "|" + movieYears[selID] +
                "|" + editedOverview + "|" + moviePosters[selID] + "|" + movieVotes[selID] + "|" + 
                movieGenres[selID] + "|";

                selMovieID = Convert.ToInt32(movieIDs[selID]);
                GetCast(movieData);
            }
        }

        //Displays Users Specific Movie Clicked From Search Results
        public void DisplayChosenMovie(int selMovieID)
        {
            /*Chosen Movie Parts
             * ------------------
             * 0 = movie ID
             * 1 = movie Title
             * 2 = movie Year
             * 3 = movie OverView
             * 4 = movie Poster
             * 5 = movie Vote
             * 6 = movie Genre
            */

            //Grab Trailer And Displays it
            GetTrailer(chosenMovieParts[0]);

            //Sets movieDisplay Area In Front Of Results List
            Canvas.SetZIndex(theMovieDisplay, 5);
            theMovieDisplay.Visibility = Visibility.Visible;

            //Sets Title and Year
            txtMovieTitle.Text = chosenMovieParts[1] + " (" + chosenMovieParts[2] + ")";

            //Sets Movie Poster
            moviePoster.Source = new BitmapImage(new Uri(this.BaseUri, chosenMovieParts[4]));

            //Sets Cast Images and Names
            imgCast0.Source = new BitmapImage(new Uri(this.BaseUri, movieCastImage[0]));
            imgCast1.Source = new BitmapImage(new Uri(this.BaseUri, movieCastImage[1]));
            imgCast2.Source = new BitmapImage(new Uri(this.BaseUri, movieCastImage[2]));
            txtCast0.Text = movieCast[0];
            txtCast1.Text = movieCast[1];
            txtCast2.Text = movieCast[2];

            //Displays OverView for Movie
            txtTheMovieOverview.Text = chosenMovieParts[3];

        }// end DisplayChosenMovie

        //Gets Trailer Of Movie Clicked
        public async void GetTrailer(string theMovieID)
        {
            var client = new HttpClient();
            HttpResponseMessage data = await client.GetAsync(
                new Uri("http://api.themoviedb.org/3/movie/" + theMovieID + "/videos?api_key=1f324761abbf02c4cdffa8505733a8c0"));
            var fullString = await data.Content.ReadAsStringAsync();

            //Add Movie Trailer
            int startPos = fullString.IndexOf("\"key\":");
            startPos += 7;
            int endPos = fullString.IndexOf(",\"", startPos);
            string url1 = "https://www.youtube.com/embed/";
            string url2 = fullString.Substring(startPos, (endPos - startPos - 1));
            string fullUrl = url1 + url2;
            trailer.Source = new Uri(fullUrl);
        }
        
        //Changes Users Rating From Int(0, 1, 2) To String For Users Viewing Later               
        private void ChangeUserRating(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            string rating = sliderRating.Value.ToString();
            switch (rating)
            {
                case "0":
                    {
                        txtRatingChose.Text = "ShipWreck";
                        showBoatRating = "ShipWreck";
                        break;
                    }
                case "1":
                    {
                        txtRatingChose.Text = "Smooth Sailing";
                        showBoatRating = "Smooth Sailing";
                        break;
                    }
                case "2":
                    {
                        txtRatingChose.Text = "Treasure";
                        showBoatRating = "Treasure";
                        break;
                    }
            }
        }

        //Navigation Menu
        private void NavigateToNewPage(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Image clickedImage = sender as Image;
            string userClicked = clickedImage.Name;

            if (userClicked == "btnNavHome")
            {
                Frame.Navigate(typeof(StartPage));
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

        //When User Clicks On Theatre Movie From StartPage
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Grabs Necessary Data So User Is Able To Add To Their Movie List
            var chosenMovie = e.Parameter as string;
            if (chosenMovie != null)
            {
                theatreMovie = true;
                //Clears needed Lists
                chosenMovieParts.Clear();
                movieCast.Clear();
                movieCastImage.Clear();

                char[] delimiterChars = { '|' };
                string[] parts;
                // seperate the movie details into Parrallel Lists
                parts = chosenMovie.Split(delimiterChars);
                chosenMovieParts.Add(parts[5]);
                chosenMovieParts.Add(parts[0]);
                //Turns the Release Date from full date to Just Year
                string editedYear = parts[1].Substring(0,4);
                chosenMovieParts.Add(editedYear);
                chosenMovieParts.Add(parts[2]);
                chosenMovieParts.Add(parts[3]);
                chosenMovieParts.Add("");
                chosenMovieParts.Add(parts[4]);
                //Grabs Cast separate Method    
                GetCast(chosenMovieParts[0]);
            }
        }

        //Displays When User Clicks Add Movie (The Rating Menu)
        private void ShowMovieRating(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            btnRating.Visibility = Visibility.Collapsed;
            spRateMovie.Visibility = Visibility.Visible;
        }

        //Adds Movie To Your List, And Saves It To Movies.txt
        private async void AddMovie(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            string dateAdded = DateTime.Now.ToString("dd/MM/yyy");
            if (checkBoxOwned.IsChecked == true)
            {
                trueOrFalse = "True";
            }
            else
            {
                trueOrFalse = "False";
            }
            if (showBoatRating == "")
            {
                showBoatRating = "Shipwreck";
            }
            //Bundles Movie Chosen For Saving To File(Movies.txt)
            movieItem = chosenMovieParts[1] + ";" + chosenMovieParts[2] +
            ";" + chosenMovieParts[6] + ";" + showBoatRating +
            ";" + userNotes.Text + ";" + dateAdded + ";" + chosenMovieParts[4] +
            ";" + chosenMovieParts[3] + ";" + trueOrFalse.ToString();
            movies.Add(movieItem);
            //Saves Movie To Your List (Movies.txt)
            var localFolder = ApplicationData.Current.LocalFolder;
            var file = await localFolder.CreateFileAsync(
                "Movies.txt", CreationCollisionOption.OpenIfExists);
            await FileIO.AppendLinesAsync(file, movies);

            Frame.Navigate(typeof(MyMovies));

        }
    } // end class
}
