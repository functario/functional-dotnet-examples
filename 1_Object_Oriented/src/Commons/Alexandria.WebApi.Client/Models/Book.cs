// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace CleanArchitecture.WebAPI.Client.Models
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class Book : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>The authorsIds property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<long?>? AuthorsIds { get; set; }
#nullable restore
#else
        public List<long?> AuthorsIds { get; set; }
#endif
        /// <summary>The id property</summary>
        public long? Id { get; set; }
        /// <summary>The publication property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::CleanArchitecture.WebAPI.Client.Models.Publication? Publication { get; set; }
#nullable restore
#else
        public global::CleanArchitecture.WebAPI.Client.Models.Publication Publication { get; set; }
#endif
        /// <summary>The title property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Title { get; set; }
#nullable restore
#else
        public string Title { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::CleanArchitecture.WebAPI.Client.Models.Book"/> and sets the default values.
        /// </summary>
        public Book()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::CleanArchitecture.WebAPI.Client.Models.Book"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::CleanArchitecture.WebAPI.Client.Models.Book CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::CleanArchitecture.WebAPI.Client.Models.Book();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "authorsIds", n => { AuthorsIds = n.GetCollectionOfPrimitiveValues<long?>()?.AsList(); } },
                { "id", n => { Id = n.GetLongValue(); } },
                { "publication", n => { Publication = n.GetObjectValue<global::CleanArchitecture.WebAPI.Client.Models.Publication>(global::CleanArchitecture.WebAPI.Client.Models.Publication.CreateFromDiscriminatorValue); } },
                { "title", n => { Title = n.GetStringValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteCollectionOfPrimitiveValues<long?>("authorsIds", AuthorsIds);
            writer.WriteLongValue("id", Id);
            writer.WriteObjectValue<global::CleanArchitecture.WebAPI.Client.Models.Publication>("publication", Publication);
            writer.WriteStringValue("title", Title);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
