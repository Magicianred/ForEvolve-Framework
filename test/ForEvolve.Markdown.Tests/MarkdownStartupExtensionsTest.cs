﻿using ForEvolve.Markdown;
using Markdig;
using Markdig.Parsers;
using Markdig.Parsers.Inlines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.Extensions.DependencyInjection
{
    public class MarkdownStartupExtensionsTest
    {
        [Fact]
        public void Should_register_default_services()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddMarkdig();

            // Assert
            AssertMarkdownConverter(services);
        }

        [Fact]
        public void Should_disable_html_by_default()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act & Assert
            services.AddMarkdig(options =>
            {
                options.Configure = builder =>
                {
                    var parser = builder.BlockParsers.Find<HtmlBlockParser>();
                    Assert.Null(parser);

                    var inlineParser = builder.InlineParsers.Find<AutolineInlineParser>();
                    Assert.False(inlineParser.EnableHtmlParsing);
                };
            });
            AssertMarkdownConverter(services);
        }

        [Fact]
        public void Should_not_disable_html_when_specified_in_options()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act & Assert
            services.AddMarkdig(options =>
            {
                options.DisableHtml = false;
                options.Configure = builder =>
                {
                    var parser = builder.BlockParsers.Find<HtmlBlockParser>();
                    Assert.NotNull(parser);

                    var inlineParser = builder.InlineParsers.Find<AutolineInlineParser>();
                    Assert.True(inlineParser.EnableHtmlParsing);
                };
            });
            AssertMarkdownConverter(services);
        }

        [Fact]
        public void Should_configure_pipeline_when_an_action_is_provided()
        {
            // Arrange
            var services = new ServiceCollection();
            bool called = false;

            // Act
            services.AddMarkdig(options => options.Configure = builder => called = true);

            // Assert
            AssertMarkdownConverter(services);
            Assert.True(called);
        }

        private static void AssertMarkdownConverter(ServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var markdownConverter = serviceProvider.GetService<IMarkdownConverter>();
            Assert.IsType<MarkdigMarkdownConverter>(markdownConverter);
        }
    }
}