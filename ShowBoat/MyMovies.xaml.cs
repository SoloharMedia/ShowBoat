using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ShowBoat
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MyMovies : Page
    {
        //Counter Variables
        int movieCounter = 0;
        int selIndex = 0;
        static int moviesOwnedCount;
        static int moviesWatchedCount;

        //Lists For Storing Movie Data
        List<string> movies = new List<string>();
        List<string> genres = new List<string>();
        List<string> titles = new List<string>();
        List<string> years = new List<string>();
        List<string> descs = new List<string>();
        List<string> posters = new List<string>();
        List<string> dates = new List<string>();
        List<string> ratings = new List<string>();
        List<string> userNotes = new List<string>();
        List<string> movieOwned = new List<string>();

        public MyMovies() // Start of MyMovies Page
        {
            this.InitializeComponent();
            LoadLetters();
            LoadGenres();
            LoadRatings();
            LoadMovies();
        }

        //Loads Filter By Letter ComboBox
        public void LoadLetters()
        {
            char[] letters = {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'K', 'L', 'M', 'N',
                            'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'};
            foreach (var letter in letters)
            {
                cbLetter.Items.Add(letter);
            }
        }

        //Loads Filter By Genre ComboBox
        public void LoadGenres()
        {
            string[] genres = {"Action", "Adventure", "Animation", "Comedy", "Crime", "Documentary",
                                "Drama", "Family", "Fantasy", "History", "Horror", "Music", "Mystery",
                                "Romance", "Science-Fiction", "TV-Movie", "Thriller", "Western"};
            foreach (var genre in genres)
            {
                cbGenre.Items.Add(genre);
            }
        }

        //Loads Filter By Rating ComboBox
        public void LoadRatings()
        {
            string[] ratings = {"ShipWrecked", "Smooth Sailing", "Treasure"};
            foreach (var rating in ratings)
            {
                cbRating.Items.Add(rating);
            }
        }
        
        //Loads All Of Users Movies From Movies.txt File
        public async void LoadMovies()
        {
            moviesWatchedCount = 0;
            moviesOwnedCount = 0;
            movies.Clear();
            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync(
                "Movies.txt", Windows.Storage.CreationCollisionOption.OpenIfExists);
            var movieFile = await FileIO.ReadLinesAsync(file);

            foreach (var movie in movieFile)
            {
                movies.Add(movie);
            }
            moviesWatchedCount = movies.Count;
            char[] delimiterChars = { ';' };
            string[] parts;
            // seperate the movie details into Parrallel Lists
            foreach (var movie in movies)
            {
                parts = movie.Split(delimiterChars);
                titles.Add(parts[0]);
                years.Add(parts[1]);
                genres.Add(parts[2]);
                ratings.Add(parts[3]);
                userNotes.Add(parts[4]);
                dates.Add(parts[5]);
                posters.Add(parts[6]);
                descs.Add(parts[7]);
                movieOwned.Add(parts[8]);
            }
            for (int i = 0; i < movies.Count; i++)
            {
                if (movieOwned[i].Equals("True"))
                {
                    moviesOwnedCount++;
                }
            }
            txtMoviesWatched.Text = "Movies Watched: " + moviesWatchedCount.ToString();
            txtMoviesOwn.Text = "Movies Owned: " + moviesOwnedCount.ToString();
            //Displays All The Movies
            for (int i = 0; i < titles.Count; i++)
            {
                Image tempMovieImage = new Image();
                tempMovieImage.Name = "userMovie" + movieCounter;
                tempMovieImage.Height = 140;
                tempMovieImage.Margin = new Thickness(4);
                tempMovieImage.Source = new BitmapImage(new Uri(this.BaseUri, posters[i]));
                movieListBaby.Items.Add(tempMovieImage);                
            }
            
        }

        //Displays Movie User Clicked On HIS / HER List
        private void DisplayUsersMovie(object sender, SelectionChangedEventArgs e)
        {
            Canvas.SetZIndex(displayedMovie, 30);
            displayedMovie.Visibility = Visibility.Visible;
            selIndex = movieListBaby.SelectedIndex;
            if (selIndex >= 0)
            {
                chosenMovieComment.Text = userNotes[selIndex];
                chosenMoviePoster.Source = new BitmapImage(new Uri(this.BaseUri, posters[selIndex]));
                chosenMovieTitle.Text = titles[selIndex];
                chosenMovieGenreYear.Text = years[selIndex] + " (" + genres[selIndex] + ")";
                txtChosenRating.Text = ratings[selIndex];
                //This Makes it Shipwreck If User Doesnt Move Slider
                if (ratings[selIndex] == "0")
                {
                    ratings[selIndex] = "Shipwreck";
                }
                switch (ratings[selIndex].ToString())
                {
                    case "Shipwreck":
                        {
                            imgChosenRating.Source = new BitmapImage(new Uri(this.BaseUri, "Assets/Images/SBShipwreckedIcon.png"));
                            break;
                        }
                    case "Smooth Sailing":
                        {
                            imgChosenRating.Source = new BitmapImage(new Uri(this.BaseUri, "Assets/Images/SBSmoothSailingIcon.png"));
                            break;
                        }
                    case "Treasure":
                        {
                            imgChosenRating.Source = new BitmapImage(new Uri(this.BaseUri, "Assets/Images/SBTreasureIcon.png"));
                            break;
                        }
                }
            }
        }

        //Displays User's Movies By First Letter Chosen
        private void DisplayByLetter(object sender, SelectionChangedEventArgs e)
        {
            if (cbLetter.SelectedIndex >= 0)
            {
                txtMyMovieSearch.Text = "";
                genres.Clear();
                years.Clear();
                titles.Clear();
                ratings.Clear();
                userNotes.Clear();
                dates.Clear();
                posters.Clear();
                descs.Clear();
                movieOwned.Clear();
                movieListBaby.Items.Clear();

                string letterChosen = cbLetter.SelectedItem.ToString();
                char[] delimiterChars = { ';' };
                string[] parts;
                for (int i = 0; i < movies.Count; i++)
                {
                    if (movies[i].Substring(0, 1) == letterChosen)
                    {
                        parts = movies[i].Split(delimiterChars);
                        titles.Add(parts[0]);
                        years.Add(parts[1]);
                        genres.Add(parts[2]);
                        ratings.Add(parts[3]);
                        userNotes.Add(parts[4]);
                        dates.Add(parts[5]);
                        posters.Add(parts[6]);
                        descs.Add(parts[7]);
                        movieOwned.Add(parts[8]);
                    }
                    else
                    {
                        //do nothing
                    }

                }

                for (int i = 0; i < titles.Count; i++)
                {
                    Image tempMovieImage = new Image();
                    tempMovieImage.Name = "userMovie" + movieCounter;
                    tempMovieImage.Height = 140;
                    tempMovieImage.Margin = new Thickness(4);
                    tempMovieImage.Source = new BitmapImage(new Uri(this.BaseUri, posters[i]));
                    movieListBaby.Items.Add(tempMovieImage);
                }
                //Puts Combo Boxes Back To Placeholding Text
                cbGenre.SelectedIndex = -1;
                cbRating.SelectedIndex = -1;
            }
            
        }

        //Closes The Specific Movie StackPanel
        private void CloseMovieChosen(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            movieListBaby.SelectedItem = null;
            displayedMovie.Visibility = Visibility.Collapsed;
            Canvas.SetZIndex(displayedMovie, 10);
        }

        //Displays User's Movies By KeyWords Typed
        private void DisplayByKeywords(object sender, TextChangedEventArgs e)
        {
            genres.Clear();
            years.Clear();
            titles.Clear();
            ratings.Clear();
            userNotes.Clear();
            dates.Clear();
            posters.Clear();
            descs.Clear();
            movieOwned.Clear();
            movieListBaby.Items.Clear();

            string usersSearch = txtMyMovieSearch.Text;
            char[] delimiterChars = { ';' };
            string[] parts;

            for (int i = 0; i < movies.Count; i++)
            {
                if (movies[i].ToLower().Contains(usersSearch) || movies[i].ToUpper().Contains(usersSearch))
                {
                    parts = movies[i].Split(delimiterChars);
                    titles.Add(parts[0]);
                    years.Add(parts[1]);
                    genres.Add(parts[2]);
                    ratings.Add(parts[3]);
                    userNotes.Add(parts[4]);
                    dates.Add(parts[5]);
                    posters.Add(parts[6]);
                    descs.Add(parts[7]);
                    movieOwned.Add(parts[8]);
                }
                else
                {
                    //do nothing
                }

            }

            for (int i = 0; i < titles.Count; i++)
            {
                Image tempMovieImage = new Image();
                tempMovieImage.Name = "userMovie" + movieCounter;
                tempMovieImage.Height = 140;
                tempMovieImage.Margin = new Thickness(4);
                tempMovieImage.Source = new BitmapImage(new Uri(this.BaseUri, posters[i]));
                movieListBaby.Items.Add(tempMovieImage);
            }
            //Puts Combo Boxes Back To Placeholding Text
            cbGenre.SelectedIndex = -1;
            cbLetter.SelectedIndex = -1;
            cbRating.SelectedIndex = -1;
        }

        //Displays Movies By Genre Selected
        private void DisplayByGenre(object sender, SelectionChangedEventArgs e)
        {
            if (cbGenre.SelectedIndex >= 0)
            {
                genres.Clear();
                years.Clear();
                titles.Clear();
                ratings.Clear();
                userNotes.Clear();
                dates.Clear();
                posters.Clear();
                descs.Clear();
                movieOwned.Clear();
                txtMyMovieSearch.Text = "";
                movieListBaby.Items.Clear();

                string userGenreChoice = cbGenre.SelectedItem.ToString();
                char[] delimiterChars = { ';' };
                string[] parts;

                int startPos = 0;
                int endPos = 0;

                for (int i = 0; i < movies.Count; i++)
                {
                    startPos = movies[i].IndexOf(';', movies[i].IndexOf(';') + 1) + 1;
                    endPos = movies[i].IndexOf(';', startPos);
                    string testGenre = movies[i].Substring(startPos, (endPos - startPos));

                    if (userGenreChoice == testGenre)
                    {
                        parts = movies[i].Split(delimiterChars);
                        titles.Add(parts[0]);
                        years.Add(parts[1]);
                        genres.Add(parts[2]);
                        ratings.Add(parts[3]);
                        userNotes.Add(parts[4]);
                        dates.Add(parts[5]);
                        posters.Add(parts[6]);
                        descs.Add(parts[7]);
                        movieOwned.Add(parts[8]);
                    }
                    else
                    {
                        //do Nothing
                    }
                }
                for (int i = 0; i < titles.Count; i++)
                {
                    Image tempMovieImage = new Image();
                    tempMovieImage.Name = "userMovie" + movieCounter;
                    tempMovieImage.Height = 140;
                    tempMovieImage.Margin = new Thickness(4);
                    tempMovieImage.Source = new BitmapImage(new Uri(this.BaseUri, posters[i]));
                    movieListBaby.Items.Add(tempMovieImage);
                }
                //Puts Combo Boxes Back To Placeholding Text
                cbLetter.SelectedIndex = -1;
                cbRating.SelectedIndex = -1;
            }            
        }

        //Displays Movies By Rating Chosen
        private void DisplayByRating(object sender, SelectionChangedEventArgs e)
        {
            if (cbRating.SelectedIndex >= 0)
            {
                genres.Clear();
                years.Clear();
                titles.Clear();
                ratings.Clear();
                userNotes.Clear();
                dates.Clear();
                posters.Clear();
                descs.Clear();
                movieOwned.Clear();
                txtMyMovieSearch.Text = "";
                movieListBaby.Items.Clear();

                string userRatingChoice = cbRating.SelectedItem.ToString();
                char[] delimiterChars = { ';' };
                string[] parts;

                for (int i = 0; i < movies.Count; i++)
                {
                    if (movies[i].Contains(userRatingChoice))
                    {
                        parts = movies[i].Split(delimiterChars);
                        titles.Add(parts[0]);
                        years.Add(parts[1]);
                        genres.Add(parts[2]);
                        ratings.Add(parts[3]);
                        userNotes.Add(parts[4]);
                        dates.Add(parts[5]);
                        posters.Add(parts[6]);
                        descs.Add(parts[7]);
                        movieOwned.Add(parts[8]);
                    }
                    else
                    {
                        //do Nothing
                    }
                }
                for (int i = 0; i < titles.Count; i++)
                {
                    Image tempMovieImage = new Image();
                    tempMovieImage.Name = "userMovie" + movieCounter;
                    tempMovieImage.Height = 140;
                    tempMovieImage.Margin = new Thickness(4);
                    tempMovieImage.Source = new BitmapImage(new Uri(this.BaseUri, posters[i]));
                    movieListBaby.Items.Add(tempMovieImage);
                }
                //Puts Combo Boxes Back To Placeholding Text
                cbLetter.SelectedIndex = -1;
                cbGenre.SelectedIndex = -1;
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
            if (userClicked == "btnNavSearch")
            {
                Frame.Navigate(typeof(MainPage));
            }
            if (userClicked == "btnNavTrivia")
            {
                Frame.Navigate(typeof(Trivia));
            }
        }

        //Removes A Movie From MyList (then rewrites entire list by Remaking Movies.txt)
        private async void RemoveMovie(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            genres.Clear();
            years.Clear();
            titles.Clear();
            ratings.Clear();
            userNotes.Clear();
            dates.Clear();
            posters.Clear();
            descs.Clear();
            movieOwned.Clear();
            movies.RemoveAt(selIndex);
            movieListBaby.Items.Clear();
            displayedMovie.Visibility = Visibility.Collapsed;

            var localFolder = ApplicationData.Current.LocalFolder;
            var file = await localFolder.CreateFileAsync(
                "Movies.txt", CreationCollisionOption.ReplaceExisting);
            await FileIO.AppendLinesAsync(file, movies);
            LoadMovies();
        }
    }//end main
}//end namespace
