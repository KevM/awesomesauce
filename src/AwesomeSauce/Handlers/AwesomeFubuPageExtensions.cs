﻿using System;
using System.Reflection;
using System.Text;
using AwesomeSauce.Configuration;
using AwesomeSauce.Handlers;
using AwesomeSauce.Views;
using FubuCore;
using FubuCore.Reflection;
using FubuMVC.Core.UI.Configuration;
using FubuMVC.Core.UI.Forms;
using FubuMVC.Core.UI.Security;
using FubuMVC.Core.UI.Tags;
using FubuMVC.Core.View;
using HtmlTags;

namespace FubuMVC.Core.UI
{
    public static class AwesomeFubuPageExtensions
    {
        public static string AwesomeUrlFor(this IFubuPage page, object model, bool isNew)
        {
            var editType = typeof (RestfulPatchRequest<>).MakeGenericType(model.GetType());
            var request = Activator.CreateInstance(editType).As<IRequestById>();
            request.Id = AwesomeConfiguration.GetIdValue(model);
            return isNew
                       ? page.Urls.UrlFor(typeof (RestfulCreateHandler<>).MakeGenericType(model.GetType()))
                       : page.Urls.UrlFor(request);
                //:typeof(RestfulPatchHandler<>).MakeGenericType(model.GetType());
            
        }

        //returning a string is DUMB
        public static string AwesomeFields(this IFubuPage page, object model)
        {
            var type = model.GetType();
            var result = new StringBuilder();
            var tags = page.Tags<AwesomeEditModel>();
            var sl = page.Get<IServiceLocator>();

            tags.SetProfile(AwesomeConfiguration.TagProfile);

            foreach(var prop in getProperties(type))
            {

                var p = new SingleProperty(prop, type);
                var elementRequest = new ElementRequest(model, p, sl);
                var accessRight = page.Get<IFieldAccessService>().RightsFor(elementRequest);
            
                var line = new FormLineExpression<AwesomeEditModel>(tags, tags.NewFieldLayout(), elementRequest)
                    .Access(accessRight)
                    .Editable(true);

                result.Append(line.ToString());
            }

            return result.ToString();
        }

        static PropertyInfo[] getProperties(Type type)
        {
            return type.GetProperties();
        }
    }
}