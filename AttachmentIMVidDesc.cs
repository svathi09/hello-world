using System;  
using System.Threading.Tasks;  
using Microsoft.Bot.Builder.Dialogs;  
using Microsoft.Bot.Connector;  
using System.IO;  
using System.Web;  
using System.Collections.Generic;  
  
namespace BotAttachment.Dialogs  
{  
    [Serializable]  
    public class AttachmentDialog : IDialog<object>  
    {    
        public async Task StartAsync(IDialogContext context)  
        {  
            context.Wait(this.MessageReceivedAsync);  
        }  
     private readonly IDictionary<string, string> MobOptions = new Dictionary<string, string>  
            {  
                {"a", "a. GOOGLE PIXEL" },
                {"b", "b. IPHONE 8" },
                //{"c", "c. ONE PLUS 5" },
            };
     private readonly IDictionary<string, string> GooglePixelAttachmentOptions = new Dictionary<string, string>            
            {   { "1", "1. Image " },  
                { "2", "2. Videos" }, 
				{ "3", "3. Description" }, 
				{ "4", "4. All" }, 
             
            };  
	 private readonly IDictionary<string, string> Iphone8AttachmentOptions = new Dictionary<string, string>            
            {   { "5", "5. Image " },  
                { "6", "6. Videos" }, 
				{ "7", "7. Description" }, 
				{ "8", "8. All" }, 
             
            };  
            public async virtual Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)  
            {  
                var message = await result;  
                var welcomeMessage = context.MakeMessage();  
                welcomeMessage.Text = "Welcome to Mobile Attachment Demo";  
              
                await context.PostAsync(welcomeMessage);  
              
                await this.DisplayOptionsAsync(context);  
            }  
              
            public async Task DisplayOptionsAsync(IDialogContext context)  
            {  
                PromptDialog.Choice<string>(  
                    context,  
                    this.MobileOptionAsync, //SelectedOptionAsync,  
                    this.MobOptions.Keys,  
                    "What Mobile option would you like to see?",  
                    "Please select Valid option a to c",  
                    6,  
                    PromptStyle.PerLine,  
                    this.MobOptions.Values);  
            }     
			public async Task DisplayPromtGooglePixelAsync(IDialogContext context)  
            {  
                PromptDialog.Choice<string>(  
                    context,  
                    this.SelectedOptionAsync,  
                    this.GooglePixelAttachmentOptions.Keys,  
                    "What Google Pixel option would you like to see?",  
                    "Please select Valid option 1 to 9",  
                    6,  
                    PromptStyle.PerLine,  
                    this.GooglePixelAttachmentOptions.Values);  
            }     
			public async Task DisplayPromtIphone8Async(IDialogContext context)  
            {  
                PromptDialog.Choice<string>(  
                    context,  
                    this.SelectedOptionAsync,
                    this.Iphone8AttachmentOptions.Keys,  
                    "What Iphone 8 option would you like to see?",  
                    "Please select Valid option 1 to 9",  
                    6,  
                    PromptStyle.PerLine,  
                    this.Iphone8AttachmentOptions.Values);  
            }     
		public async Task MobileOptionAsync(IDialogContext context, IAwaitable<string> argument)  
          {   var message = await argument;  
               switch (message)  
            {  
                case "a":  
                    await this.DisplayPromtGooglePixelAsync(context);
                    break;  
                case "b":  
                    await this.DisplayPromtIphone8Async(context);
                    break;  
			}
		  }			
          public async Task SelectedOptionAsync(IDialogContext context, IAwaitable<string> argument)  
          {  
            var message = await argument;  
          
            var replyMessage = context.MakeMessage();  
              
            Attachment attachment = null;  
            
                  
            switch (message)  
            {  
                case "1":  
                    attachment = GetGooglePixelImageAttachment();  
                    replyMessage.Text = "Attach Image of Google Pixel";  //
					ShowListOutput(context,attachment,replyMessage);
                    break;  
                case "5":  
                    attachment = GetIphone8ImageAttachment();  
                    replyMessage.Text = "Attach Image of Iphone8";  //
					ShowListOutput(context,attachment,replyMessage);
                    break;  
                case "2":  
                    attachment = GetGooglePixelVideoAttachment();  
                    replyMessage.Text = "Attach Video of Google Pixel";
					ShowListOutput(context,attachment,replyMessage);  
                    break;  
                case "6":  
                    attachment = GetIphone8VideoAttachment();  
                    replyMessage.Text = "Attach Video of IPhone8";  
					ShowListOutput(context,attachment,replyMessage);
                    break;  
                case "3":  
                    attachment = GetGooglePixelDescriptionAttachment();  
                    replyMessage.Text = "Attach File of GooglePixel";  //
					ShowListOutput(context,attachment,replyMessage);
                    break;  
                case "7":  
                    attachment = GetIphone8DescriptionAttachment();  
                    replyMessage.Text = "Attach File of Iphone8";  //
					ShowListOutput(context,attachment,replyMessage);
                    break;  
                case "4":   
                    replyMessage.Text = "Attach All details GooglePixel";  //
                    var result1 = GetGooglePixelAllAttachment();
                    ShowCombinedListOutput(context,result1.Item1,result1.Item2,result1.Item3,replyMessage);
                    //  ShowListOutput(context,attachment.Item1,replyMessage);
					//  ShowListOutput(context,attachment.Item2,replyMessage);
					//  ShowListOutput(context,attachment.Item3,replyMessage);
                    break;   
                case "8":      
                    replyMessage.Text = "Attach All details of Iphone8";  //
                    var result2 = GetIphone8AllAttachment();
                    ShowCombinedListOutput(context,result2.Item1,result2.Item2,result2.Item3,replyMessage);
					//  ShowListOutput(context,attachment.Item1,replyMessage);
					//  ShowListOutput(context,attachment.Item2,replyMessage);
					//  ShowListOutput(context,attachment.Item3,replyMessage);
                    break;  
          
            }  
            
         }  
         /// <summary>  
        /// dispaly local image  
        /// </summary>  
        /// <returns></returns>  
		private async Task ShowListOutput(IDialogContext context, Attachment attachment, IMessageActivity replyMessage)
		{
			replyMessage.Attachments = new List<Attachment> { attachment };  
            
          
            context.PostAsync(replyMessage);  
          
            this.DisplayOptionsAsync(context);  
		}
        private async Task ShowCombinedListOutput(IDialogContext context,Attachment attachment1,Attachment attachment2,Attachment attachment3,IMessageActivity replyMessage)
		{
			replyMessage.Attachments = new List<Attachment> {attachment1,attachment2,attachment3};  
            
            /*await */ context.PostAsync(replyMessage);  
          
            /*await */ this.DisplayOptionsAsync(context);  
		}
        private static Attachment GetGooglePixelImageAttachment()  
        {  
            return new Attachment  
            {  
               Name = "googlepixel.png",  
               ContentType = "image/jpg",  
               ContentUrl = "https://cdn2.gsmarena.com/vv/pics/google/google-pixel-02.jpg"  
            };  
        }  
        private static Attachment GetIphone8ImageAttachment()  
        {  
            return new Attachment  
            {  
                Name = "iphone8.jpg",  
                ContentType = "image/jpg",  
                ContentUrl = "https://cdn2.gsmarena.com/vv/pics/apple/apple-iphone-8-new-1.jpg"  
            };  
        }  
        
        
        public static Attachment GetGooglePixelVideoAttachment()  
        {  
            Attachment attachment = new Attachment();  
            attachment.ContentType = "video/mp4";  
            attachment.ContentUrl = "https://www.youtube.com/watch?v=XGFQOZ_owtc";  
            return attachment;  
        } 
        public static Attachment GetIphone8VideoAttachment()  
        {  
            Attachment attachment = new Attachment();  
            attachment.ContentType = "video/mp4";  
            attachment.ContentUrl = "https://www.youtube.com/watch?v=5es64FPCO6k";  
            return attachment;  
        }  
        public static Attachment GetGooglePixelDescriptionAttachment()  
        {  
            Attachment attachment = new Attachment();  
            attachment.ContentType = "application/pdf";  
            attachment.ContentUrl = "https://www.1000ordi.ch/google-pixel-xl-32-gb-%28black%29-google-pixelxl-32gb-black-103351_fr.pdf";  
            attachment.Name = "Google Pixel Description PDF";  
            return attachment;
        } 
		public static Attachment GetIphone8DescriptionAttachment()  
        {  
            Attachment attachment = new Attachment();  
            attachment.ContentType = "application/pdf";  
            attachment.ContentUrl = "https://images.apple.com/environment/pdf/products/iphone/iPhone_8_PER_sept2017.pdf";  
            attachment.Name = "Iphone 8 Description PDF";  
            return attachment;
        } 
        public static Tuple<Attachment,Attachment,Attachment>GetGooglePixelAllAttachment()
        {
            var a=GetGooglePixelImageAttachment();
            var b= GetGooglePixelVideoAttachment();
            var c= GetGooglePixelDescriptionAttachment();
            var result= Tuple.Create<Attachment,Attachment,Attachment>(a,b,c);
            return result;
        }
        public static Tuple<Attachment,Attachment,Attachment>GetIphone8AllAttachment()
        {
            var a = GetIphone8ImageAttachment();
            var b= GetIphone8VideoAttachment();
            var c= GetIphone8DescriptionAttachment();
            var result= Tuple.Create<Attachment,Attachment,Attachment>(a,b,c);
            return result;
        }
         
         
    }
}  
    