﻿/*
This source file is subject to version 3 of the GPL license, 
that is bundled with this package in the file LICENSE, and is 
available online at http://www.gnu.org/licenses/gpl.txt; 
you may not use this file except in compliance with the License. 

Software distributed under the License is distributed on an "AS IS" basis,
WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
the specific language governing rights and limitations under the License.

All portions of the code written by Whoaverse are Copyright (c) 2014 Whoaverse
All Rights Reserved.
*/

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Whoaverse.Models;

namespace Whoaverse.Controllers
{
    public class WebApiController : ApiController
    {
        private whoaverseEntities db = new whoaverseEntities();

        // GET api/defaultsubverses
        /// <summary>
        ///  This API returns a list of default subverses shown to guests.
        /// </summary>
        [System.Web.Http.HttpGet]
        public IEnumerable<string> DefaultSubverses()
        {
            var listOfDefaultSubverses = db.Defaultsubverses.OrderBy(s => s.position).ToList();

            List<string> tmpList = new List<string>();
            foreach (var item in listOfDefaultSubverses)
            {
                tmpList.Add(item.name);
            }

            return tmpList;
        }

        // GET api/bannedhostnames
        /// <summary>
        ///  This API returns a list of banned hostnames for link type submissions.
        /// </summary>
        [System.Web.Http.HttpGet]
        public IEnumerable<string> BannedHostnames()
        {
            var bannedHostnames = db.Banneddomains.OrderBy(s => s.Added_on).ToList();

            List<string> tmpList = new List<string>();
            foreach (var item in bannedHostnames)
            {
                tmpList.Add("Hostname: " + item.Hostname + ", reason: " + item.Reason + ", added on: " + item.Added_on + ", added by: " + item.Added_by);
            }

            return tmpList;
        }

        // GET api/top200subverses
        /// <summary>
        ///  This API returns top 200 subverses ordered by subscriber count.
        /// </summary>
        [System.Web.Http.HttpGet]
        public IEnumerable<string> Top200Subverses()
        {
            var top200Subverses = db.Subverses.OrderByDescending(s => s.subscribers).ToList();

            List<string> resultList = new List<string>();
            foreach (var item in top200Subverses)
            {
                resultList.Add(
                    "Name: " + item.name + "," +
                    "Description: " + item.description + "," +
                    "Subscribers: " + item.subscribers + "," +
                    "Created: " + item.creation_date
                    );
            }

            return resultList;
        }

        // GET api/frontpage
        /// <summary>
        ///  This API returns 100 submissions which are currently shown on WhoaVerse frontpage.
        /// </summary>
        [System.Web.Http.HttpGet]
        public IEnumerable<string> Frontpage()
        {
            //get only submissions from default subverses, order by rank
            var frontpageSubmissions = (from message in db.Messages
                                        join defaultsubverse in db.Defaultsubverses on message.Subverse equals defaultsubverse.name
                                        where message.Name != "deleted"
                                        select message)
                               .Distinct()
                               .OrderByDescending(s => s.Rank).Take(100).ToList();

            List<string> resultList = new List<string>();
            foreach (var item in frontpageSubmissions)
            {
                resultList.Add(
                    "Type: " + item.Type + "," +
                    "Title: " + item.Title + "," +
                    "Link description: " + item.Linkdescription + "," +
                    "Subverse: " + item.Subverse + "," +
                    "Date: " + item.Date + "," +
                    "Comments: " + item.Comments.Count() + "," +
                    "Author: " + item.Name
                    );
            }

            return resultList;
        }

        // GET api/subversefrontpage
        /// <summary>
        ///  This API returns 100 submissions which are currently shown on frontpage of a given subverse.
        /// </summary>
        /// <param name="subverse">The name of the subverse for which to fetch submissions.</param>
        [System.Web.Http.HttpGet]
        public IEnumerable<string> SubverseFrontpage(string subverse)
        {
            //get only submissions from given subverses, order by rank
            var frontpageSubmissions = (from message in db.Messages
                                        where message.Name != "deleted" && message.Subverse == subverse
                                        select message)
                               .Distinct()
                               .OrderByDescending(s => s.Rank).Take(100).ToList();

            if (frontpageSubmissions == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            List<string> resultList = new List<string>();
            foreach (var item in frontpageSubmissions)
            {
                resultList.Add(
                    "Type: " + item.Type + "," +
                    "Title: " + item.Title + "," +
                    "Link description: " + item.Linkdescription + "," +
                    "Date: " + item.Date + "," +
                    "Comments: " + item.Comments.Count() + "," +
                    "Author: " + item.Name
                    );
            }

            return resultList;
        }

        // GET api/singlesubmission
        /// <summary>
        ///  This API returns a single submission for a given submission ID.
        /// </summary>
        /// <param name="id">The ID of submission to fetch.</param>
        [System.Web.Http.HttpGet]
        public Message SingleSubmission(int id)
        {            
            Message submission = db.Messages.Find(id);
            if (submission == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            Message tmpResult = new Message();
            tmpResult.Id = submission.Id;
            tmpResult.Date = submission.Date;
            tmpResult.LastEditDate = submission.LastEditDate;
            tmpResult.Likes = submission.Likes;
            tmpResult.Dislikes = submission.Dislikes;
            tmpResult.Rank = submission.Rank;
            tmpResult.Thumbnail = submission.Thumbnail;
            tmpResult.Subverse = submission.Subverse;
            tmpResult.Type = submission.Type;
            tmpResult.Title = submission.Title;
            tmpResult.Linkdescription = submission.Linkdescription;
            tmpResult.MessageContent = submission.MessageContent;
            return tmpResult;           
        }

        // GET api/singlecomment
        /// <summary>
        ///  This API returns a single comment for a given comment ID.
        /// </summary>
        /// <param name="id">The ID of comment to fetch.</param>
        [System.Web.Http.HttpGet]
        public Comment SingleComment(int id)
        {
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            Comment tmpResult = new Comment();
            tmpResult.Id = comment.Id;
            tmpResult.Date = comment.Date;
            tmpResult.LastEditDate = comment.LastEditDate;
            tmpResult.Likes = comment.Likes;
            tmpResult.Dislikes = comment.Dislikes;
            tmpResult.CommentContent = comment.CommentContent;
            tmpResult.ParentId = comment.ParentId;
            tmpResult.MessageId = comment.MessageId;
            tmpResult.Name = comment.Name;

            return tmpResult;
        }
        
    }
}