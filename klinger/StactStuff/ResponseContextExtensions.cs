﻿// Copyright 2007-2010 The Apache Software Foundation.
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
namespace klinger.StactStuff
{
    using Client;
    using Stact.ServerFramework;

    public static class ResponseContextExtensions
    {
        static readonly SparkRender _render = new SparkRender();

        public static void RenderSparkView(this ResponseContext cxt, EnvironmentValidatorRepository repo, string template)
        {
            string output = _render.Render("status.html", repo.TakeTemperature(), new KlingerInformation());
            cxt.WriteHtml(output);
        }
    }
}