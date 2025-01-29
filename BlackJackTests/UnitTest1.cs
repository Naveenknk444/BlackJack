  // Assert
            Assert.That(extracted, Is.Not.Null, "Extracted HallexRecord should not be null for a valid ID.");

            Assert.Multiple(() =>
            {
                Assert.That(extracted.LinkId, Is.EqualTo(validId), "LinkId does not match expected value.");
                Assert.That(extracted.MarkupContentText, Is.Not.Null, "MarkupContentText should not be null.");
                Assert.That(extracted.PolicyNetObjectTypeCode, Is.EqualTo(PolicyNetObjectTypeCodes.HEARINGS_APPEALS_AND_LITIGATION_LAW_LEX_MANUAL),
                    "PolicyNetObjectTypeCode does not match expected value.");
                Assert.That(extracted.filename, Is.Not.Null.And.Not.Empty, "Filename should not be null or empty.");
                Assert.That(extracted.action, Is.EqualTo("version"), "Action should match expected value.");
                Assert.That(extracted.lastUpdated, Is.GreaterThan(DateTime.MinValue), "LastUpdated should have a valid timestamp.");
                Assert.That(extracted.@type, Is.EqualTo("section"), "Type should match expected value.");
            });
