//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using BeJournalist.Resources;

using Microsoft.WindowsAzure.MobileServices;
using System.Net.NetworkInformation;

namespace BeJournalist
{
    
    public partial class MainPage : PhoneApplicationPage
    {
        // TODO : N'oubliez pas de changer le microsoft ID de l'administrateur !
        //----------------------------------------------------------------------
        private string MY_MICROSOFT_ID = "MicrosoftAccount:15ebaa53ba0c8a3f1ce7d0e3fa4d94c8";


        // MobileServiceCollectionView implements ICollectionView (useful for databinding to lists) and 
        // is integrated with your Mobile Service to make it easy to bind your data to the ListView
        private MobileServiceCollectionView<CommentItem> items;
        private IMobileServiceTable<CommentItem> commentTable;

        private bool auth = false;

        // Constructeur
        public MainPage()
        {
            InitializeComponent();
            commentTable = App.MobileService.GetTable<CommentItem>();
            
        }

        /// <summary>
        /// Permet de rafraichir la liste des commentaires.
        /// </summary>
        /// <returns>void.</returns>
        private void RefreshCommentsItems()
        {
            try
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                { 

                    // On interroge ici la tale commentTable pour sortir les élements voulus.
                    // La requete trie les CommentItems par Id
                    items = commentTable
                        .OrderByDescending(x => x.Id)
                        .ToCollectionView();
                    ListItems.ItemsSource = items;
                }
                else
                {
                    MessageBox.Show("Cette application a besoin d'un accès internet.");
                }

                
            }
            catch (Exception)
            {
                MessageBox.Show("Cette application nécessite un accès internet.");
            }
            
        }

        /// <summary>
        /// Permet de lancer l'authentification asynchrone pour accéder à la partie Admin 
        /// </summary>
        /// <returns>Un objet tâche .</returns>
        private async System.Threading.Tasks.Task Authenticate()
        {
            string message = "";
            try
            {
                //Si la personne n'est pas déjà auth, on lance la requete
                if (!auth)
                    await App.MobileService.LoginAsync(MobileServiceAuthenticationProvider.MicrosoftAccount);

                // Quoi qu'il en soit, on vérifie son ID
                if (App.MobileService.CurrentUser.UserId.Equals(MY_MICROSOFT_ID))
                {
                    message = string.Format("Bienvenue dans votre espace !");
                    auth = true;
                }
                else
                    message = "Désolé, ce compte n'est pas administrateur";

            }
            catch (InvalidOperationException)
            {
                message = "Désolé, la section admin est privée.";
            }

            MessageBox.Show(message);

            // Si l'auth a réussie, on accède à la page admin
            if (auth)
            {
                this.NavigationService.Navigate(new Uri("/Admin.xaml", UriKind.Relative));
            }

        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            RefreshCommentsItems();
        }

        // Clic sur le bouton refresh de l'application bar
        private void refresh_Click(object sender, EventArgs e)
        {
            RefreshCommentsItems();
        }

        // Clic sur le bouton admin de l'application bar
        private async void admin_Click(object sender, EventArgs e)
        {
            await Authenticate();   
        }
    }
}