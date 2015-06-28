﻿using System;
using System.Windows;
using System.Collections.ObjectModel;
using AMP.GeoCachingTools.Model;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using System.Collections.Generic;

namespace AMP.GeoCachingTools.ViewModel
{
    public class TextBoxViewModel : BaseINPC
    {
        List<string> longitudeList = new List<string>();

        List<string> latitudeList = new List<string>();

        public TextBoxViewModel()
        {
            this.fillLists();
        }

        private void fillLists()
        {
            longitudeList.Add("N50°25.123'");
            longitudeList.Add("N50.418716°");
            longitudeList.Add("N50°25' 07.4''");

            latitudeList.Add("E006°45.000'");
            latitudeList.Add("E006.750000°");
            latitudeList.Add("E006°45' 00.0''");
        }

    }
}