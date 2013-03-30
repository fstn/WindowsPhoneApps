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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.WindowsAzure.MobileServices;

namespace BeJournalist
{
    public partial class Admin : PhoneApplicationPage
    {
        private MobileServiceCollectionView<CommentItem> items;
        private IMobileServiceTable<CommentItem> commentTable = App.MobileService.GetTable<CommentItem>();

        public Admin()
        {
            InitializeComponent();

            this.Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshComments();
        }
        
        //Clic sur l'ajout d'une commentaire
        private void Badd_Click(object sender, RoutedEventArgs e)
        {
            //On crée un objet commentaire que l'on rempli avec le champ texte, et on le rajoute à la base.
            CommentItem comment = new CommentItem { Contenu = TextComment.Text, Sport = "Tennis", Match = "Nadal - federer" };
            InsertCommentItem(comment);
        }

        /// <summary>
        /// Permet de rajouter un commentaire dans notre liste et dans Azure, de facon asynchrone.
        /// </summary>
        /// <returns>void.</returns>
        private async void InsertCommentItem(CommentItem commentItem)
        {

            try
            {
                await commentTable.InsertAsync(commentItem);

                items.Add(commentItem);

                MessageBox.Show("Ce commentaire a bien été ajouté.");
                TextComment.Text = "";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }

            RefreshComments();
        }


        /// <summary>
        /// Permet de rafraichir la liste de commentaires.
        /// </summary>
        /// <returns>void.</returns>
        private void RefreshComments()
        {
            // This code refreshes the entries in the list view be querying the TodoItems table.
            items = commentTable
                .OrderByDescending(x => x.Id)
                .ToCollectionView();
            ListItems.ItemsSource = items;
        }

        /// <summary>
        /// Permet de supprimer un commentaire de la base Azure, de facon asynchrone
        /// </summary>
        /// <returns>void.</returns>
        private async void DeleteComment(CommentItem bi)
        {
            try{
                await this.commentTable.DeleteAsync(bi);

                MessageBox.Show("Ce commentaire a bien été supprimé.");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }

            RefreshComments();

        }

        /// <summary>
        /// Permet de lancer la suppression d'un commentaire lorsque l'administrateur clique sur un commentaire de la liste.
        /// </summary>
        /// <returns>void.</returns>
        private void ListItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBoxResult dialogR = MessageBox.Show("Etes-vous sûr de vouloir supprimer ce commentaire ?", "Suppression", MessageBoxButton.OKCancel);
            if (dialogR == MessageBoxResult.OK)
            {
                CommentItem ci = (CommentItem)e.AddedItems[0];
                DeleteComment(ci);
            }
        }

        private void TextComment_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TextComment.Text.Equals("Votre commentaire sportif ici..."))
                TextComment.Text = "";
        }
    }
}