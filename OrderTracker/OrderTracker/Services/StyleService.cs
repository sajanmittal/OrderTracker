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

            var TextInput = new Style(typeof(Entry))
            {
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

            var Form = new Style(typeof(StackLayout))
            {

                Setters =
                {
                    new Setter{Property = View.HorizontalOptionsProperty, Value = LayoutOptions.FillAndExpand},
                    new Setter{Property  = View.VerticalOptionsProperty, Value= LayoutOptions.Start},
                    new Setter{Property= StackLayout.OrientationProperty, Value= StackOrientation.Vertical},
                    new Setter{Property = Layout.PaddingProperty, Value = new Thickness(5,10)}
                }
            };
            Add(Form, nameof(Form));

            var FormBlock = new Style(typeof(StackLayout))
            {

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

            var ListViewSubCell = new Style(typeof(StackLayout))
            {
                Setters =
                {
                    new Setter{Property = View.HorizontalOptionsProperty, Value = LayoutOptions.FillAndExpand},
                    new Setter{Property  = View.VerticalOptionsProperty, Value= LayoutOptions.StartAndExpand},
                    new Setter{Property= StackLayout.OrientationProperty, Value= StackOrientation.Horizontal}
                }
            };
            Add(ListViewSubCell, nameof(ListViewSubCell));

            var BaseButton = new Style(typeof(Button))
            {
                Setters =
                {
                    new Setter{Property= View.HorizontalOptionsProperty, Value= LayoutOptions.CenterAndExpand},
                    new Setter{Property  = View.VerticalOptionsProperty, Value= LayoutOptions.Center},
                    new Setter{Property= VisualElement.BackgroundColorProperty, Value= Color.Accent},
                    new Setter{Property= Button.TextColorProperty, Value= Color.White},
                    new Setter{Property= View.MarginProperty, Value= new Thickness(10)},
                    new Setter{Property= View.WidthRequestProperty, Value = 200}
                }
            };
            Add(BaseButton, nameof(BaseButton));

            var Date = new Style(typeof(DatePicker))
            {
                Setters =
                {
                    new Setter{Property = View.HorizontalOptionsProperty, Value = LayoutOptions.FillAndExpand},
                    new Setter{Property  = View.VerticalOptionsProperty, Value= LayoutOptions.CenterAndExpand}
                }
            };
            Add(Date, nameof(Date));

            return resources;

        }

        private static void Add<T>(T data, string name)
        {
            resources.Add(name, data);
        }

    }
}
