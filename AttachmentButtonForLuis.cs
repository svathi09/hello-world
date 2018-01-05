using System;  
using System.Threading.Tasks;  
using Microsoft.Bot.Builder.Dialogs;  
using Microsoft.Bot.Connector;  
using System.IO;  
using System.Web;  
using System.Collections.Generic;  
  

using System.Collections.Generic;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
//using CardsBot;

namespace BotAttachment.Dialogs  
{   
    [LuisModel("fb168e36-d3a3-411d-9b30-5645657dc142","1c2ff22856c242d5b0d33ed731fb421f", LuisApiVersion.V2,"westus.api.cognitive.microsoft.com")]
      //luis id for svComparePhone3
    [Serializable]  
    public class AttachmentDialogButton : IDialog<object>  
    {   
        private const string EntityPhone1 = "Google Pixel";
        private const string EntityPhone2 = "Iphone8";
        //private const string EntityPhone3 = "oneplus5";
        EntityRecommendation GooglePixelRecommendation;
        EntityRecommendation Iphone8Recommendation;
        //EntityRecommendation Oneplus5Recommendation;
        
        //EntityRecommendation QuestionEntityRecommendation;
        EntityRecommendation PhoneEntityRecommendation;
        //EntityRecommendation SpecsEntityRecommendation;
        
  
        
         
       public async Task StartAsync(IDialogContext context)  
        {  
            //await context.PostAsync("16");  
            context.Wait(this.MessageReceivedAsync);  
        }    
       public async virtual Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)  
            {   //await context.PostAsync("20");  
                var message = await result;  
                var welcomeMessage = context.MakeMessage();  
                welcomeMessage.Text = "Welcome to Mobile Attachment Demo- Type any query regarding any mobile";  
                await context.PostAsync(welcomeMessage);  
                context.Wait(LuisIntent_qna_redirect3);  
            }      
                
                [LuisIntent("qna_redirect3")] ////////////?
                public async Task LuisIntent_qna_redirect3(IDialogContext context, LuisResult result)
                {
                    string message = "qna_redirect3 Intent Detected";
                    await context.PostAsync(message);
                    string Detections = "" ;
                    
                    //Show all Entities in the utterance
                    for(int i =0;i<result.Entities.Count;i++)
                    {
                        Detections+= i.ToString()+". Entity: " + result.Entities[i].Entity + " ; Type: "+result.Entities[i].Type +"\n" ;
                        Detections = Detections.Replace("\n",System.Environment.NewLine);
                    } 
                   await context.PostAsync(Detections); 
              
             if (result.TryFindEntity("Phone_Type::GooglePixel", out GooglePixelRecommendation))
             {
                //Dialogs.AttachmentDialogButton AttachButton = new Dialogs.AttachmentDialogButton();
                context.Wait(DisplayPromtGooglePixelAsync);
                
                //if (!AttachButton.TryQuery(PhoneEntityRecommendation.Entity, out message)){
                   // message = $"Sorry, I do not know'{PhoneEntityRecommendation.Entity}'";
                //}
                
                //await context.PostAsync(message);
             }
             
              else if (result.TryFindEntity("Phone_Type::Iphone8", out Iphone8Recommendation))
              {
                 //Dialogs.AttachmentDialogButton AttachButton = new Dialogs.AttachmentDialogButton();
                 context.Wait(DisplayPromtIphone8Async);
                              
                //if (!AttachButton.TryQuery(PhoneEntityRecommendation.Entity, out message)){
                   // message = $"Sorry, I do not know'{PhoneEntityRecommendation.Entity}'";
              //}
               //await context.PostAsync(message);
             } 
             else 
             { 
                context.Wait(OptionPromtMob);
             }
            context.Done(1);    // Go back to Root
                
            }
        //  [LuisIntent("")]
        //  [LuisIntent("None")]
        //  public async Task None(IDialogContext context, LuisResult result)
        //   { 
            
        //      string message = $"Sorry, I do not know'{result.Query}'";

        //      await context.PostAsync("Please ask a question of form <Question type-Specification-Phone Model>");
            
                
        //      await context.PostAsync(message);
            
        //      context.Done(1);    // Go back to Root
        //  }
            
            
            
        //////////////////////////////////
        private const string GooglePixel = "Google Pixel";
        private const string Iphone8 = "Iphone 8";
        //for mob choice button
        private async Task OptionPromtMob(IDialogContext context,IAwaitable<IMessageActivity> argument)
            {
                PromptDialog.Choice(
                    context, 
                    this.MobileOptionAsync, 
                    new[] {GooglePixel,Iphone8}, 
                    "Which Mobile detail would you like to know about", 
                    "I am sorry but I didn't understand that. I need you to select one of the options below",
                    attempts: 2);
            }

        public async Task MobileOptionAsync(IDialogContext context, IAwaitable<string> argument)  
          {   var message = await argument;  
               switch (message)  
            {  
                case "Google Pixel":  
                    //await this.DisplayPromtGooglePixelAsync(context);//,argument);
                    context.Wait(DisplayPromtGooglePixelAsync);
                    break;  
                case "Iphone 8":  
                    //await this.DisplayPromtIphone8Async(context);//,argument);
                    context.Wait(DisplayPromtIphone8Async);
                    break;  
            }
        }	
       //for google pixel button
       private const string Image1 = "Image1";       
       private const string Video1 = "Video1";
       private const string Description1 = "Description1";
       private const string All1 = "All1";
       
       private async Task DisplayPromtGooglePixelAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
            {
                PromptDialog.Choice(
                    context, 
                    this.SelectedMobileOption, 
                    new[] {Image1, Video1,Description1,All1}, 
                    "Choose one of the Google Pixel details", 
                    "I am sorry but I didn't understand that. I need you to select one of the options below",
                    attempts: 2);
            }	
       //for iphone 8 button
       private const string Image2 = "Image2";
       private const string Video2 = "Video2";
       private const string Description2 = "Description2";
       private const string All2= "All2";
       
       private async Task DisplayPromtIphone8Async(IDialogContext context, IAwaitable<IMessageActivity> argument)
            {
                PromptDialog.Choice(
                    context, 
                    this.SelectedMobileOption, 
                    new[] {Image2, Video2,Description2,All2}, 
                    "Choose one of the Iphone 8 details", 
                    "I am sorry but I didn't understand that. I need you to select one of the options below",
                    attempts: 2);
            }	
        //option cases google pixel(1)  and iphone 8(2)   
        public async Task SelectedMobileOption(IDialogContext context, IAwaitable<string> argument)  
          {  
            var message = await argument;  
          
            var replyMessage = context.MakeMessage();  
              
            Attachment attachment = null;  
            
                  
            switch (message)  
            {  
                case "Image1":  
                    attachment = GetGooglePixelImageAttachment();  
                    replyMessage.Text = "Attach Image of Google Pixel";  //
					ShowListOutput(context,attachment,replyMessage,argument);
                    break;  
                case "Video1":  
                    attachment = GetGooglePixelVideoAttachment();  
                    replyMessage.Text = "Attach Video of Google Pixel";
					ShowListOutput(context,attachment,replyMessage,argument);  
                    break;  
                case "Description1":  
                    attachment = GetGooglePixelDescriptionAttachment();  
                    replyMessage.Text = "Attach File of GooglePixel";  //
					ShowListOutput(context,attachment,replyMessage,argument);
                    break;  
                case "All1":   
                    replyMessage.Text = "Attach All details GooglePixel";  //
                    var result1 = GetGooglePixelAllAttachment();
                    ShowCombinedListOutput(context,result1.Item1,result1.Item2,result1.Item3,replyMessage,argument);
                    break; 
                case "Image2":  
                    attachment = GetIphone8ImageAttachment();  
                    replyMessage.Text = "Attach Image of Iphone8";  //
					ShowListOutput(context,attachment,replyMessage,argument);
                    break;  
                case "Video2":  
                    attachment = GetIphone8VideoAttachment();  
                    replyMessage.Text = "Attach Video of IPhone8";  
					ShowListOutput(context,attachment,replyMessage,argument);
                    break;  
                case "Description2":  
                    attachment = GetIphone8DescriptionAttachment();  
                    replyMessage.Text = "Attach File of Iphone8";  //
					ShowListOutput(context,attachment,replyMessage,argument);
                    break;     
                case "All2":      
                    replyMessage.Text = "Attach All details of Iphone8";  //
                    var result2 = GetIphone8AllAttachment();
                    ShowCombinedListOutput(context,result2.Item1,result2.Item2,result2.Item3,replyMessage,argument);
                    break;  
            }  
            
         }  
         
       //////////////////function to Display as Msg
       private async Task ShowListOutput(IDialogContext context, Attachment attachment, IMessageActivity replyMessage,IAwaitable<string> argument)
		{   var message = await argument;  
			replyMessage.Attachments = new List<Attachment> { attachment };  
            
            context.PostAsync(replyMessage);  
            //this.OptionPromtMob(context);//,argument);  
            context.Wait(OptionPromtMob);
		}
        private async Task ShowCombinedListOutput(IDialogContext context,Attachment attachment1,Attachment attachment2,Attachment attachment3,IMessageActivity replyMessage,IAwaitable<string> argument)
		{   var message = await argument;  
			replyMessage.Attachments = new List<Attachment> {attachment1,attachment2,attachment3};  
            
            /*await */ context.PostAsync(replyMessage);  
           // /*await */ this.OptionPromtMob(context);//,argument);  
            context.Wait(OptionPromtMob);
		}
        
        //////////////function Im Vid Desc All
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