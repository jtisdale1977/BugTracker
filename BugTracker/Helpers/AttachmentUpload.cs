using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public class AttachmentUpload
    {
        public static bool WebFriendlyImage(HttpPostedFileBase files)
        {
            if(files == null)
                return false;

            if (files.ContentLength > 2 * 1024 * 1024 || files.ContentLength < 1024)
                return false;

            else
                return true;
        }
        
    }
}