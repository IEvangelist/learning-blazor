// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Learning.Blazor.Models
{
    /// <summary>
    /// See: https://haveibeenpwned.com/api/v3#BreachModel
    /// </summary>
    public sealed class BreachDetails : BreachHeader
    {
        /// <summary>
        /// A descriptive title for the breach suitable for displaying to end users. It's unique across all breaches but individual values may change in the future (i.e. if another breach occurs against an organisation already in the system). If a stable value is required to reference the breach, refer to the "Name" attribute instead.
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// The domain of the primary website the breach occurred on. This may be used for identifying other assets external systems may have for the site.
        /// </summary>
        public string Domain { get; set; } = null!;

        /// <summary>
        /// The date (with no time) the breach originally occurred on in ISO 8601 format. This is not always accurate — frequently breaches are discovered and reported long after the original incident. Use this attribute as a guide only.
        /// </summary>
        public DateTime BreachDate { get; set; }

        /// <summary>
        /// The date and time (precision to the minute) the breach was added to the system in ISO 8601 format.
        /// </summary>
        public DateTime AddedDate { get; set; }

        /// <summary>
        /// The date and time (precision to the minute) the breach was modified in ISO 8601 format. This will only differ from the AddedDate attribute if other attributes represented here are changed or data in the breach itself is changed (i.e. additional data is identified and loaded). It is always either equal to or greater then the AddedDate attribute, never less than.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// The total number of accounts loaded into the system. This is usually less than the total number reported by the media due to duplication or other data integrity issues in the source data.
        /// </summary>
        public int PwnCount { get; set; }

        /// <summary>
        /// Contains an overview of the breach represented in HTML markup. The description may include markup such as emphasis and strong tags as well as hyperlinks.
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// This attribute describes the nature of the data compromised in the breach and contains an alphabetically ordered string array of impacted data classes.
        /// </summary>
        public string[] DataClasses { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Indicates that the breach is considered unverified. An unverified breach may not have been hacked from the indicated website. An unverified breach is still loaded into HIBP when there's sufficient confidence that a significant portion of the data is legitimate.
        /// </summary>
        public bool IsVerified { get; set; }

        /// <summary>
        /// Indicates that the breach is considered fabricated. A fabricated breach is unlikely to have been hacked from the indicated website and usually contains a large amount of manufactured data. However, it still contains legitimate email addresses and asserts that the account owners were compromised in the alleged breach.
        /// </summary>
        public bool IsFabricated { get; set; }

        /// <summary>
        /// Indicates if the breach is considered sensitive. The public API will not return any accounts for a breach flagged as sensitive.
        /// </summary>
        public bool IsSensitive { get; set; }

        /// <summary>
        /// Indicates if the breach has been retired. This data has been permanently removed and will not be returned by the API.
        /// </summary>
        public bool IsRetired { get; set; }

        /// <summary>
        /// Indicates if the breach is considered a spam list. This flag has no impact on any other attributes but it means that the data has not come as a result of a security compromise.
        /// </summary>
        public bool IsSpamList { get; set; }

        /// <summary>
        /// A URI that specifies where a logo for the breached service can be found. Logos are always in PNG format.
        /// </summary>
        public string LogoPath { get; set; } = null!;
    }
}
