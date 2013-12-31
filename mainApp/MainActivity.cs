using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Text;
using IO.Card.Payment;

namespace mainApp
{
	[Activity (Label = "mainApp", MainLauncher = true)]
	public class MainActivity : Activity
	{
		private static string CARDIO_TOKEN = "put_your_token_here";

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);
			
			button.Click += delegate {
				var scanIntent = new Intent(this, typeof (CardIOActivity));

				scanIntent.PutExtra(CardIOActivity.ExtraAppToken, CARDIO_TOKEN);

				scanIntent.PutExtra(CardIOActivity.ExtraRequireExpiry, true); 	
				scanIntent.PutExtra(CardIOActivity.ExtraRequireCvv, true); 		
				scanIntent.PutExtra(CardIOActivity.ExtraRequirePostalCode, true); 
				scanIntent.PutExtra(CardIOActivity.ExtraUseCardioLogo, false);

				StartActivityForResult(scanIntent, 100);

			};
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);

			if (data != null && data.HasExtra(CardIOActivity.ExtraScanResult))
			{
				var scanResult = data.GetParcelableExtra(CardIOActivity.ExtraScanResult).JavaCast<CreditCard>();
				RunOnUiThread(() =>
					{
						var sb = new StringBuilder();

						sb.AppendLine("card number " + scanResult.CardNumber);
						sb.AppendLine("cvv " + scanResult.Cvv);
						sb.AppendLine("exipry " + scanResult.ExpiryMonth.ToString() + "/" + scanResult.ExpiryYear.ToString());
						sb.AppendLine("zip " + scanResult.Zip);

						var ad = new AlertDialog.Builder(this);
						ad.SetTitle("New card added");
						ad.SetMessage(sb.ToString());
						ad.SetPositiveButton("OK", delegate
							{
								ad.Dispose();
							});
						ad.Show();
					});
			}
		}
	}
}


