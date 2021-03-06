﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AMP.GeoCachingTools.ViewModel;
using AMP.GeoCachingTools;
using Windows.Devices.Geolocation;
using Windows.UI.Popups;
using AMP.GeoCachingTools.Commons;

namespace AMP.GeoCachingTools.View
{

    public sealed partial class PeilenPage : Page
    {

        private BaseViewModel bvm;

        private string messageDialogTitle;

        private string messageDialogContent;

        public PeilenPage()
        {
            init();
        }

        // Initialize the components; own method because of deletion function in the UI
        private void init()
        {
            InitializeComponent();

            bvm = new BaseViewModel();

            //Set the data context of the window
            DataContext = bvm;

            NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void calculatePosition(object sender, RoutedEventArgs e)
        {
            bvm.calculatePosition();

            if (bvm.exception != null)
            {
                // Exceptionhandling for empty fields
                if (bvm.exception.Message.Equals("emptyFields"))
                {
                    string dialogHelper;
                    string dialogHelperVerb;

                    if (bvm.validator.emptyFields.Count == 1)
                    {
                        dialogHelper = "Das Feld ";
                        dialogHelperVerb = " ist ";

                        foreach (string field in bvm.validator.emptyFields)
                        {
                            dialogHelper = dialogHelper + field;
                        }
                    }
                    else
                    {
                        dialogHelper = "Die Felder ";
                        dialogHelperVerb = " sind ";

                        int length = bvm.validator.emptyFields.Count - 1;

                        for (int counter = 0; counter <= length; counter++)
                        {
                            if (counter < length - 1)
                            {
                                dialogHelper = dialogHelper + bvm.validator.emptyFields[counter] + ", ";
                            }
                            else if (counter == length - 1)
                            {
                                dialogHelper = dialogHelper + bvm.validator.emptyFields[counter] + " und ";
                            }
                            else
                            {
                                dialogHelper = dialogHelper + bvm.validator.emptyFields[counter];
                            }

                        }
                    }

                    messageDialogTitle = "Ungültige Parametrisierung!";
                    messageDialogContent = dialogHelper + dialogHelperVerb + "leer. Bitte geben Sie einen Wert ein.";
                }
                // Exceptionhandling for wrong values like '2132dsfd'
                else if (bvm.exception.Message.Equals("wrongValue"))
                {
                    messageDialogTitle = "Ungültiger Wert!";
                    messageDialogContent = "Sie haben einen ungültigen Wert eingegeben. Bitte geben Sie eine Zahl im Format 'x,xx' ein.";
                }
                else if (bvm.exception.Message.StartsWith("Degrees"))
                {
                    messageDialogTitle = "Ungültiges Format der Geokoordinaten!";
                    messageDialogContent = "Sie haben ungültige Geokoordinaten eingegeben. Bitte geben Sie Koordinaten im Format ";

                    if (bvm.exception.Message.Equals("DegreesMinutes"))
                    {
                        messageDialogContent += Constants.DegreesMinutes + " ein.";
                    }
                    else if (bvm.exception.Message.Equals("Degrees"))
                    {
                        messageDialogContent += Constants.Degrees + " ein.";
                    }
                    else if (bvm.exception.Message.Equals("DegreesMinutesSeconds"))
                    {
                        messageDialogContent += Constants.DegreesMinutesSeconds + " ein.";
                    }
                }
                // Exceptionhandling for Degrees in direction between 0° and 360°
                else if (bvm.exception.Message.Equals("notInRange"))
                {
                    messageDialogTitle = "Nicht im Wertebereich!";
                    messageDialogContent = "Sie haben einen ungültigen Wert für Richtung eingegeben. Bitte geben Sie eine Zahl zwischen 0 und 360 ein.";
                }

                createMessageDialog(messageDialogContent, messageDialogTitle);
            }
        }


        private void getPosition(object sender, RoutedEventArgs e)
        {
            bvm.getPosition();

            // Exceptionhandling for disabled locationsettings or other errors
            if (bvm.exception != null)
            {
                if (bvm.exception.Message.Equals("LocationSettingIsDisabled"))
                {
                    messageDialogTitle = "Ortung inaktiv!";
                    messageDialogContent = "Bitte aktivieren Sie die Ortung in den Einstellungen.";
                }
                else
                {
                    messageDialogTitle = "Fehler bei Ortung!";
                    messageDialogContent = "Es ist ein Fehler bei der Ortung aufgetreten.";
                }

                createMessageDialog(messageDialogContent, messageDialogTitle);
            }

        }

        private void delete(object sender, RoutedEventArgs e)
        {
            init();
        }

        private void CommandHandlers(IUICommand commandLabel)
        {
            var Actions = commandLabel.Label;
            switch (Actions)
            {
                case "Ok":
                    // nothing to do
                    break;
            }
        }

        private async void createMessageDialog(string messageDialogContent, string messageDialogTitle)
        {
            MessageDialog msg = new MessageDialog(messageDialogContent, messageDialogTitle);

            msg.Commands.Add(new UICommand("Ok", new UICommandInvokedHandler(CommandHandlers)));

            await msg.ShowAsync();
        }

        private void changeCoordinates(object sender, SelectionChangedEventArgs e)
        {
            bvm.changeCoordinates();
        }
    }
}
