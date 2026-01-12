using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.DTO.CommentDto
{
    public class CommentDetailsResponse : CommentResponse
    {
  
        public CommentResponse? ParentComment { get; set; }
        public List<CommentResponse> Replies { get; set; } = new();
    }

}
