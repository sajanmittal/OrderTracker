using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace OrderTracker
{
    public class StyleService
    {
        private static readonly ResourceDictionary resources;

        static StyleService()
        {
            if (resources == null)
                resources = new ResourceDictionary();
        }

        public static ResourceDictionary InitializeResources()
        {

            var TextInput = new Style(typeof(Entry)) { 
                 Setters =
                {
                    new Setter{Property = View.HorizontalOptionsProperty, Value = LayoutOptions.FillAndExpand},
                    new Setter{Property  = View.VerticalOptionsProperty, Value= LayoutOptions.CenterAndExpand}
                }
            };
            Add(TextInput, nameof(TextInput));

            var TextLabel = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{Property = View.HorizontalOptionsProperty, Value = LayoutOptions.Start},
                    new Setter{Property  = View.VerticalOptionsProperty, Value= LayoutOptions.CenterAndExpand},
                    new Setter{Property = Label.FontSizeProperty, Value = 14 },
                    new Setter{Property = Label.FontAttributesProperty, Value = FontAttributes.Bold}
                }
            };
            Add(TextLabel, nameof(TextLabel));

            var Form = new Style(typeof(StackLayout)) { 

                Setters=
                {
                    new Setter{Property = View.HorizontalOptionsProperty, Value = LayoutOptions.FillAndExpand},
                    new Setter{Property  = View.VerticalOptionsProperty, Value= LayoutOptions.Start},
                    new Setter{Property= StackLayout.OrientationProperty, Value= StackOrientation.Vertical},
                    new Setter{Property = Layout.PaddingProperty, Value = new Thickness(5,10)}
                }
            };
            Add(Form, nameof(Form));

            var FormBlock = new Style(typeof(StackLayout)) { 

                Setters =
                {
                    new Setter{Property = View.HorizontalOptionsProperty, Value = LayoutOptions.FillAndExpand},
                    new Setter{Property  = View.VerticalOptionsProperty, Value= LayoutOptions.FillAndExpand},
                    new Setter{Property= StackLayout.OrientationProperty, Value= StackOrientation.Horizontal},
                    new Setter{Property= View.MarginProperty, Value= new Thickness(10)}
                }
            };
            Add(FormBlock, nameof(FormBlock));

            var ListViewCell = new Style(typeof(StackLayout))
            {
                Setters =
                {
                    new Setter{Property = View.HorizontalOptionsProperty, Value = LayoutOptions.FillAndExpand},
                    new Setter{Property  = View.VerticalOptionsProperty, Value= LayoutOptions.FillAndExpand},
                    new Setter{Property= StackLayout.OrientationProperty, Value= StackOrientation.Vertical}
                }
            };
            Add(ListViewCell, nameof(ListViewCell));

            var BaseButton = new Style(typeof(Button))
            {
                Setters =
                {
                    new Setter{Property= View.HorizontalOptionsProperty, Value= LayoutOptions.Center},
                    new Setter{Property  = View.VerticalOptionsProperty, Value= LayoutOptions.Center},
                    new Setter{Property= VisualElement.BackgroundColorProperty, Value= Color.AliceBlue},
                    new Setter{Property= View.MarginProperty, Value= new Thickness(10)}
                }
            };
            Add(BaseButton, nameof(BaseButton));

            return resources;

        }

        private static void Add<T>(T data, string name)
        {
            resources.Add(name, data);
        }

    }
}
