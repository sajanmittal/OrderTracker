using System;
using System.Linq.Expressions;
using OrderTracker.Converters;
using OrderTracker.Views.Controls;
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
			ImplicitStyles();
			ExplicitStyles();
			AddConverters();
			return resources;
		}

		private static void AddConverters()
		{
			AddConverter("Toggle", new ToggleConverter());
			AddConverter("Null2Bool", new NullValueBoolConverter());
			AddConverter("PastDate", new PastRecordConverter());
		}

		private static void ImplicitStyles()
		{
			var materialDesignBase = new Style(typeof(VisualElement))
			{
				Setters =
				{
					Setter(VisualElement.VisualProperty, VisualMarker.Material)
				}
			};

			AddStyle(new Style(typeof(Entry))
			{
				Setters =
					 {
						Setter(View.HorizontalOptionsProperty, LayoutOptions.FillAndExpand),
						Setter(View.VerticalOptionsProperty, LayoutOptions.EndAndExpand),
						Setter(VisualElement.BackgroundColorProperty, Color.Transparent),
						Setter(Entry.PlaceholderColorProperty, Color.Accent)
					 },
				BasedOn = materialDesignBase
			}
			);

			AddStyle(new Style(typeof(ActivityIndicator))
			{
				Setters =
					{
						Setter(VisualElement.BackgroundColorProperty, Color.Transparent),
						Setter(ActivityIndicator.ColorProperty, Color.Accent),
						Setter(View.VerticalOptionsProperty, LayoutOptions.CenterAndExpand),
						Setter(AbsoluteLayout.LayoutBoundsProperty, new Rectangle(0.5, 0, 100, 100)),
						Setter(AbsoluteLayout.LayoutFlagsProperty, AbsoluteLayoutFlags.PositionProportional)
					},
				BasedOn = materialDesignBase
			});

			AddStyle(new Style(typeof(Label))
			{
				BasedOn = materialDesignBase,
				Setters =
				{
					Setter(View.HorizontalOptionsProperty,LayoutOptions.StartAndExpand),
					Setter(View.VerticalOptionsProperty, LayoutOptions.CenterAndExpand),
				}
			});

			AddStyle(new Style(typeof(PassDatePicker))
			{
				Setters =
					 {
						Setter(View.HorizontalOptionsProperty, LayoutOptions.FillAndExpand),
						Setter(View.VerticalOptionsProperty, LayoutOptions.CenterAndExpand),
						Setter(DatePicker.DateProperty, DateTime.Now),
						Setter(DatePicker.FormatProperty, Constants.DATE_FRMT),
						Setter(DatePicker.TextColorProperty, Color.Accent),
						Setter(VisualElement.BackgroundColorProperty, Color.Transparent)
					 },
				BasedOn = materialDesignBase
			});

			AddStyle(new Style(typeof(Button))
			{
				Setters =
					 {
						Setter(View.HorizontalOptionsProperty, LayoutOptions.CenterAndExpand),
						Setter(View.VerticalOptionsProperty,  LayoutOptions.Center),
						Setter(VisualElement.BackgroundColorProperty, Color.Accent),
						Setter(Button.TextColorProperty, Color.White),
						Setter(View.MarginProperty, new Thickness(10)),
						Setter(VisualElement.WidthRequestProperty, 200)
					 },
				BasedOn = materialDesignBase
			});

			AddStyle(new Style(typeof(Editor))
			{
				Setters =
					 {
						Setter(View.HorizontalOptionsProperty, LayoutOptions.FillAndExpand),
						Setter(View.VerticalOptionsProperty, LayoutOptions.EndAndExpand),
						Setter(VisualElement.BackgroundColorProperty, Color.Transparent),
						Setter(Editor.PlaceholderColorProperty, Color.Accent),
						Setter(Editor.AutoSizeProperty, EditorAutoSizeOption.TextChanges)
					 },
				BasedOn = materialDesignBase
			}
			);

			AddStyle(new Style(typeof(AbsoluteLayout))
			{
				Setters =
				{
					Setter(View.VerticalOptionsProperty, LayoutOptions.FillAndExpand)
				},
				BasedOn = materialDesignBase
			});

			AddStyle(new Style(typeof(CheckBox))
			{
				Setters =
					{
						Setter(View.HorizontalOptionsProperty, LayoutOptions.CenterAndExpand),
						Setter(View.VerticalOptionsProperty, LayoutOptions.CenterAndExpand),
						Setter(CheckBox.ColorProperty, Color.Accent)
					},
				BasedOn = materialDesignBase
			});

			AddStyle(new Style(typeof(SearchBar))
			{
				Setters =
					{
						Setter(View.HorizontalOptionsProperty, LayoutOptions.FillAndExpand),
						Setter(View.VerticalOptionsProperty, LayoutOptions.StartAndExpand),
						Setter(SearchBar.CancelButtonColorProperty, Color.Accent),
						Setter(SearchBar.PlaceholderColorProperty, Color.Accent)
					},
				BasedOn = materialDesignBase
			});

			AddStyle(new Style(typeof(Picker))
			{
				Setters =
					 {
						Setter(View.HorizontalOptionsProperty, LayoutOptions.FillAndExpand),
						Setter(View.VerticalOptionsProperty, LayoutOptions.EndAndExpand),
						Setter(VisualElement.BackgroundColorProperty, Color.Transparent),
						Setter(Picker.TitleColorProperty, Color.Accent)
					 },
				BasedOn = materialDesignBase
			}
			);
		}

		private static void ExplicitStyles()
		{
			var TextLabel = new Style(typeof(Label))
			{
				Setters =
					 {
						new Setter{Property = Label.FontSizeProperty, Value = 14 },
						new Setter{Property = Label.FontAttributesProperty, Value = FontAttributes.Bold}
					 }
			};
			Add(() => TextLabel);

			var TextLabelEnd = new Style(typeof(Label))
			{
				BasedOn = TextLabel,
				Setters =
						{
							new Setter{Property = View.HorizontalOptionsProperty, Value= LayoutOptions.EndAndExpand},
							new Setter{Property=View.MarginProperty, Value= new Thickness(10,0)}
						}
			};
			Add(() => TextLabelEnd);

			var Form = new Style(typeof(StackLayout))
			{
				Setters =
					 {
						new Setter{Property = View.HorizontalOptionsProperty, Value = LayoutOptions.FillAndExpand},
						new Setter{Property  = View.VerticalOptionsProperty, Value= LayoutOptions.FillAndExpand},
						new Setter{Property= StackLayout.OrientationProperty, Value= StackOrientation.Vertical},
						new Setter{Property = Layout.PaddingProperty, Value = new Thickness(5,10)},
						Setter(AbsoluteLayout.LayoutBoundsProperty, new Rectangle(0, 0, 1, AbsoluteLayout.AutoSize)),
						Setter(AbsoluteLayout.LayoutFlagsProperty, AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional)
					 }
			};
			Add(() => Form);

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
			Add(() => FormBlock);

			var ListViewBase = new Style(typeof(ListView))
			{
				Setters =
					 {
						new Setter{Property= View.HorizontalOptionsProperty, Value= LayoutOptions.FillAndExpand},
						new Setter{Property  = View.VerticalOptionsProperty, Value= LayoutOptions.FillAndExpand},
						new Setter{Property= View.MarginProperty, Value= new Thickness(20)},
						new Setter{Property= ListView.HasUnevenRowsProperty, Value= false},
						new Setter{Property= ListView.SeparatorVisibilityProperty, Value = false},
						new Setter{Property= ListView.IsPullToRefreshEnabledProperty, Value= false},
						new Setter{Property= ListView.SeparatorColorProperty, Value= Color.Transparent}
					 }
			};

			Add(() => ListViewBase);

			var ListViewLayout = new Style(typeof(ListView))
			{
				Setters =
				{
					new Setter { Property = ListView.RowHeightProperty, Value = 110 },
					Setter(AbsoluteLayout.LayoutBoundsProperty, new Rectangle(0, 0, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize)),
					Setter(AbsoluteLayout.LayoutFlagsProperty, AbsoluteLayoutFlags.PositionProportional)
				},
				BasedOn = ListViewBase
			};
			Add(() => ListViewLayout);

			var ListViewFrame = new Style(typeof(Frame))
			{
				Setters =
					 {
						new Setter{Property  = Frame.HasShadowProperty, Value= true},
						new Setter{Property= Frame.CornerRadiusProperty, Value= 3},
						new Setter{Property = Layout.PaddingProperty, Value= new Thickness(5)}
					 }
			};
			Add(() => ListViewFrame);

			var FormScroll = new Style(typeof(ScrollView))
			{
				Setters =
					{
						new Setter{Property = View.HorizontalOptionsProperty, Value = LayoutOptions.FillAndExpand},
						new Setter{Property  = View.VerticalOptionsProperty, Value= LayoutOptions.FillAndExpand},
						Setter(AbsoluteLayout.LayoutBoundsProperty, new Rectangle(0, 0, 1, AbsoluteLayout.AutoSize)),
						Setter(AbsoluteLayout.LayoutFlagsProperty, AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional)
					}
			};
			Add(() => FormScroll);

			var AppLabel = new Style(typeof(Label))
			{
				Setters =
				{
					Setter(View.HorizontalOptionsProperty,LayoutOptions.StartAndExpand),
					Setter(View.VerticalOptionsProperty, LayoutOptions.CenterAndExpand),
					Setter(Label.FontAttributesProperty, FontAttributes.Bold),
					Setter(Label.FontSizeProperty, 16)
				}
			};

			Add(() => AppLabel);

			var AbsSubmit = new Style(typeof(Button))
			{
				Setters ={
						new Setter{Property = View.HorizontalOptionsProperty, Value = LayoutOptions.CenterAndExpand},
						new Setter{Property  = View.VerticalOptionsProperty, Value= LayoutOptions.CenterAndExpand},
						Setter(AbsoluteLayout.LayoutBoundsProperty, new Rectangle(.5,1,.5,.1)),
						Setter(AbsoluteLayout.LayoutFlagsProperty, AbsoluteLayoutFlags.All),
						Setter(VisualElement.VisualProperty, VisualMarker.Material)
				}
			};

			Add(() => AbsSubmit);

			var AbsLayout = new Style(typeof(StackLayout))
			{
				Setters ={
						new Setter{Property = View.HorizontalOptionsProperty, Value = LayoutOptions.FillAndExpand},
						new Setter{Property  = View.VerticalOptionsProperty, Value= LayoutOptions.Fill},
						Setter(AbsoluteLayout.LayoutBoundsProperty, new Rectangle(0,1, 1,.2)),
						Setter(AbsoluteLayout.LayoutFlagsProperty, AbsoluteLayoutFlags.All),
						Setter(VisualElement.VisualProperty, VisualMarker.Material)
				}
			};

			Add(() => AbsLayout);

			var AbsGrid = new Style(typeof(Grid))
			{
				Setters ={
						new Setter{Property = View.HorizontalOptionsProperty, Value = LayoutOptions.FillAndExpand},
						new Setter{Property  = View.VerticalOptionsProperty, Value= LayoutOptions.FillAndExpand},
						Setter(AbsoluteLayout.LayoutBoundsProperty, new Rectangle(0, 0, 1, AbsoluteLayout.AutoSize)),
						Setter(AbsoluteLayout.LayoutFlagsProperty, AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional),
						Setter(VisualElement.VisualProperty, VisualMarker.Material)
				}
			};

			Add(() => AbsGrid);
		}

		private static void Add(Expression<Func<Style>> style)
		{
			var styleInfo = style.GetMemberInfo();
			if (!resources.ContainsKey(styleInfo.Name))
				resources.Add(styleInfo.Name, style?.Compile().Invoke());
		}

		private static void AddStyle(Style style)
		{
			if (!resources.ContainsKey(style.TargetType.FullName))
			{
				resources.Add(style);
			}
		}

		private static void AddConverter(string key, IValueConverter converter)
		{
			if (!resources.ContainsKey(key))
			{
				resources.Add(key, converter);
			}
		}

		private static Setter Setter(BindableProperty property, object value) => new Setter { Property = property, Value = value };
	}
}