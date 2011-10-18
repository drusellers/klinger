// Copyright 2007-2010 The Apache Software Foundation.
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace klinger.Server.Http.StactStuff
{
    using System;
    using System.IO;
    using System.Text;
    using klinger.Server;
    using Spark;
    using Spark.FileSystem;

    public class SparkRender
    {
        readonly EmbeddedViewFolder _viewFolder;
        readonly SparkViewEngine _engine;

        public SparkRender()
        {
            _viewFolder = new EmbeddedViewFolder(typeof (SparkRender).Assembly, "klinger.views");

            _engine = new SparkViewEngine
                          {
                              DefaultPageBaseType = typeof (VoteView).FullName,
                              ViewFolder = _viewFolder
                          };
        }

        public string Render(string template, ValidationVote[] votes, KlingerInformation info)
        {
            var view = (VoteView) _engine.CreateInstance(
                new SparkViewDescriptor()
                    .AddTemplate(template));

            view.Model = new HealthModel
                             {
                                 Votes = votes,
                                 Information = info
                             };

            var sb = new StringBuilder();

            using (var writer = new StringWriter(sb))
            {
                view.RenderView(writer);
            }

            return sb.ToString();
        }
    }

    public abstract class VoteView :
        AbstractSparkView
    {
        public HealthModel Model { get; set; }
    }

    public class HealthModel
    {
        public ValidationVote[] Votes { get; set; }
        public string CurrentTime
        {
            get { return DateTime.Now.ToString("MMM dd, yyyy"); }
        }

        public KlingerInformation Information { get; set; }
    }
}