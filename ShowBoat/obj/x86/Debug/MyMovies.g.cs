﻿#pragma checksum "C:\Users\hardy\OneDrive\Documents\Visual Studio 2015\Projects\ShowBoat\ShowBoat\MyMovies.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "C3DFBD1A8D271C128F7AAF98FDAC9DCD"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShowBoat
{
    partial class MyMovies : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.displayedMovie = (global::Windows.UI.Xaml.Controls.Border)(target);
                    #line 50 "..\..\..\MyMovies.xaml"
                    ((global::Windows.UI.Xaml.Controls.Border)this.displayedMovie).Tapped += this.CloseMovieChosen;
                    #line default
                }
                break;
            case 2:
                {
                    this.btnNavTrivia = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 116 "..\..\..\MyMovies.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.btnNavTrivia).Tapped += this.NavigateToNewPage;
                    #line default
                }
                break;
            case 3:
                {
                    this.btnNavList = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 4:
                {
                    this.btnNavSearch = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 108 "..\..\..\MyMovies.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.btnNavSearch).Tapped += this.NavigateToNewPage;
                    #line default
                }
                break;
            case 5:
                {
                    this.btnNavHome = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 104 "..\..\..\MyMovies.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.btnNavHome).Tapped += this.NavigateToNewPage;
                    #line default
                }
                break;
            case 6:
                {
                    this.chosenMoviePoster = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 7:
                {
                    this.btnRemoveFromList = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 87 "..\..\..\MyMovies.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.btnRemoveFromList).Tapped += this.RemoveMovie;
                    #line default
                }
                break;
            case 8:
                {
                    this.chosenMovieComment = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 9:
                {
                    this.chosenMovieTitle = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 10:
                {
                    this.chosenMovieGenreYear = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 11:
                {
                    this.imgChosenRating = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 12:
                {
                    this.txtChosenRating = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 13:
                {
                    this.movieListBaby = (global::Windows.UI.Xaml.Controls.GridView)(target);
                    #line 47 "..\..\..\MyMovies.xaml"
                    ((global::Windows.UI.Xaml.Controls.GridView)this.movieListBaby).SelectionChanged += this.DisplayUsersMovie;
                    #line default
                }
                break;
            case 14:
                {
                    this.txtMyMovieSearch = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    #line 33 "..\..\..\MyMovies.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.txtMyMovieSearch).TextChanged += this.DisplayByKeywords;
                    #line default
                }
                break;
            case 15:
                {
                    this.txtMoviesWatched = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 16:
                {
                    this.txtMoviesOwn = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 17:
                {
                    this.cbLetter = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                    #line 25 "..\..\..\MyMovies.xaml"
                    ((global::Windows.UI.Xaml.Controls.ComboBox)this.cbLetter).SelectionChanged += this.DisplayByLetter;
                    #line default
                }
                break;
            case 18:
                {
                    this.cbGenre = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                    #line 27 "..\..\..\MyMovies.xaml"
                    ((global::Windows.UI.Xaml.Controls.ComboBox)this.cbGenre).SelectionChanged += this.DisplayByGenre;
                    #line default
                }
                break;
            case 19:
                {
                    this.cbRating = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                    #line 29 "..\..\..\MyMovies.xaml"
                    ((global::Windows.UI.Xaml.Controls.ComboBox)this.cbRating).SelectionChanged += this.DisplayByRating;
                    #line default
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

