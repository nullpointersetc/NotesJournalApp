﻿#region "NoteEntityTests.cs"
#pragma warning disable IDE0001, IDE0057, IDE0130

namespace NullPointersEtc.NotesJournalApp.NotesDomain_Tests
{
    using Note = NullPointersEtc.NotesJournalApp.NoteEntity.Note;

    using NoteIdIsNotSetException =
        NullPointersEtc.NotesJournalApp.NoteEntity.NoteIdIsNotSetException;

    using NoteTitleIsNotSetException =
        NullPointersEtc.NotesJournalApp.NoteEntity.NoteTitleIsNotSetException;

    using NoteTitleIsEmptyException =
        NullPointersEtc.NotesJournalApp.NoteEntity.NoteTitleIsEmptyException;

    using NoteTitleIsTooLongException =
        NullPointersEtc.NotesJournalApp.NoteEntity.NoteTitleIsTooLongException;

    using NoteBodyIsNotSetException =
        NullPointersEtc.NotesJournalApp.NoteEntity.NoteBodyIsNotSetException;

    using NoteBodyIsEmptyException =
        NullPointersEtc.NotesJournalApp.NoteEntity.NoteBodyIsEmptyException;

    using NoteBodyIsTooLongException =
        NullPointersEtc.NotesJournalApp.NoteEntity.NoteBodyIsTooLongException;

    using NoteLastModifiedDateIsNotSetException =
        NullPointersEtc.NotesJournalApp.NoteEntity.NoteLastModifiedDateIsNotSetException;

    using NoteLastModifiedDateCannotGoBackInTimeException =
        NullPointersEtc.NotesJournalApp.NoteEntity.NoteLastModifiedDateCannotGoBackInTimeException;

    using NoteCreationDateIsNotSetException =
        NullPointersEtc.NotesJournalApp.NoteEntity.NoteCreationDateIsNotSetException;

    using NoteCreationDateIsNotModifiableException =
        NullPointersEtc.NotesJournalApp.NoteEntity.NoteCreationDateIsNotModifiableException;

    using Assert_t = Xunit.Assert;
    using Fact_a = Xunit.FactAttribute;

    public class NoteEntityTests
    {
        [Fact_a]
        public void CannotGetNoteIdBeforeSet()
        {
            Note note = new();

            Assert_t.Throws<NoteIdIsNotSetException>(
                () => { return note.NoteID; });
        }

        [Fact_a]
        public void CanGetNoteIdAfterSet()
        {
            Note note = new();

            System.Guid guid = new(a: 0x9bab1dde,
                b: 0x3976, c: 0x4a98,
                d: 0x8f, e: 0xcb, f: 0x2d, g: 0xe1,
                h: 0xd9, i: 0x3e, j: 0x94, k: 0xef);

            note.NoteID = guid;
            Assert_t.Equal<System.Guid>(guid, note.NoteID);
        }

        [Fact_a]
        public void CannotGetTitleBeforeSet()
        {
            Note note = new();

            Assert_t.Throws<NoteTitleIsNotSetException>(
                () => note.Title);
        }

        [Fact_a]
        public void CanGetTitleAfterSet()
        {
            Note note = new();

            note.Title = TheHoundOfTheBaskervilles.TheLegend.PostScript;

            Assert_t.Equal(expected: TheHoundOfTheBaskervilles.TheLegend.PostScript,
                actual: note.Title);
        }

        [Fact_a]
        public void SettingNullTitleThrowsException()
        {
            Note note = new();

            Assert_t.Throws<NoteTitleIsEmptyException>(
                () => note.Title = null!);
        }

        [Fact_a]
        public void SettingEmptyTitleThrowsException()
        {
            Note note = new();

            Assert_t.Throws<NoteTitleIsEmptyException>(
                () => note.Title = string.Empty);
        }

        [Fact_a]
        public void SettingTitleToSpacesThrowsException()
        {
            Note note = new();

            Assert_t.Throws<NoteTitleIsEmptyException>(
                () => note.Title = "\u0020\u0020");
        }

        [Fact_a]
        public void SettingTooLongTitleThrowsException()
        {
            Note note = new();

            Assert.Throws<NoteTitleIsTooLongException>(
                () => note.Title =
                    TheHoundOfTheBaskervilles.TheLegend.First.Substring(
                        startIndex: 0, length: Note.MAX_TITLE_LENGTH + 1));
        }

        [Fact_a]
        public void CannotGetBodyBeforeSet()
        {
            Note note = new();
            Assert.Throws<NoteBodyIsNotSetException>(() => note.Body);
        }

        [Fact_a]
        public void CanGetBodyAfterSet()
        {
            Note note = new();
            note.Body = TheHoundOfTheBaskervilles.TheLegend.Second;

            Assert_t.Equal(expected: TheHoundOfTheBaskervilles.TheLegend.Second,
                actual: note.Body);
        }

        [Fact_a]
        public void SettingNullBodyThrowsException()
        {
            Note note = new();

            Assert.Throws<NoteBodyIsEmptyException>(
                () => note.Body = null!);
        }

        [Fact_a]
        public void SettingEmptyBodyThrowsException()
        {
            Note note = new();
            Assert.Throws<NoteBodyIsEmptyException>(
                () => note.Body = string.Empty);
        }

        [Fact_a]
        public void SettingBodyToSpacesThrowsException()
        {
            Note note = new();

            Assert.Throws<NoteBodyIsEmptyException>(
                () => note.Body = "\u0020\u0020");
        }

        [Fact_a]
        public void SettingTooLongBodyThrowsException()
        {
            Note note = new();

            string longBody = TheHoundOfTheBaskervilles.TheLegend.Second +
                TheHoundOfTheBaskervilles.TheLegend.Third +
                TheHoundOfTheBaskervilles.TheLegend.Fourth +
                TheHoundOfTheBaskervilles.TheLegend.Fifth;

            Assert_t.True(longBody.Length > Note.MAX_BODY_LENGTH);

            Assert_t.Throws<NoteBodyIsTooLongException>(
                () => note.Body = longBody);

            Assert_t.Throws<NoteBodyIsTooLongException>(
                () => note.Body = longBody.Substring(
                    startIndex: 0, length: Note.MAX_BODY_LENGTH + 1));
        }

        [Fact_a]
        public void CannotGetCreatedAtBeforeSet()
        {
            Note note = new();

            Assert.Throws<NoteCreationDateIsNotSetException>(() => note.CreatedAt);
        }

        [Fact_a]
        public void CanGetCreatedAtAfterSet()
        {
            System.DateTime testDate = new(
                year: 2026, month: 7, day: 1,
                hour: 12, minute: 34, second: 56,
                millisecond: 789);

            Note note = new();
            note.CreatedAt = testDate;

            Assert_t.Equal(expected: testDate,
                actual: note.CreatedAt);
        }

        [Fact_a]
        public void CanSetCreatedAtToSameDateTime()
        {
            System.DateTime testDate = new(
                year: 2026, month: 7, day: 1,
                hour: 13, minute: 34, second: 56,
                millisecond: 789);

            System.DateTime testDate2 = new(
                year: 2026, month: 7, day: 1,
                hour: 13, minute: 34, second: 56,
                millisecond: 789);

            Note note = new();
            note.CreatedAt = testDate;

            Assert_t.Equal(expected: testDate,
                actual: note.CreatedAt);

            note.CreatedAt = testDate2;

            Assert_t.Equal(expected: testDate2,
                actual: note.CreatedAt);
        }

        [Fact_a]
        public void CannotSetCreatedAtToDifferentDateTime()
        {
            System.DateTime testDate = new(
                year: 2026, month: 7, day: 1,
                hour: 13, minute: 34, second: 56,
                millisecond: 789);

            System.DateTime testDate2 = new(
                year: 2026, month: 7, day: 1,
                hour: 15, minute: 34, second: 56,
                millisecond: 789);

            Note note = new();
            note.CreatedAt = testDate;

            Assert_t.Equal(expected: testDate,
                actual: note.CreatedAt);

            Assert_t.Throws<NoteCreationDateIsNotModifiableException>(
                () => note.CreatedAt = testDate2);

            Assert_t.Equal(expected: testDate, actual: note.CreatedAt);
        }

        [Fact_a]
        public void CannotGetUpdatedAtBeforeSet()
        {
            Note note = new();

            Assert.Throws<NoteLastModifiedDateIsNotSetException>(
                () => note.LastUpdatedAt);
        }

        [Fact_a]
        public void CanGetUpdatedAtAfterSet()
        {
            System.DateTime testDate = new(
                year: 2026, month: 7, day: 1,
                hour: 13, minute: 34, second: 56,
                millisecond: 789);

            Note note = new();
            note.LastUpdatedAt = testDate;

            Assert.Equal(expected: testDate,
                actual: note.LastUpdatedAt);
        }

        [Fact_a]
        public void CanSetUpdatedAtToSameDateTime()
        {
            System.DateTime testDate = new(
                year: 2026, month: 7, day: 1,
                hour: 13, minute: 34, second: 56,
                millisecond: 789);

            System.DateTime testDate2 = new(
                year: 2026, month: 7, day: 1,
                hour: 13, minute: 34, second: 56,
                millisecond: 789);

            Note note = new();
            note.LastUpdatedAt = testDate;

            Assert.Equal(expected: testDate,
                actual: note.LastUpdatedAt);

            note.LastUpdatedAt = testDate2;

            Assert.Equal(expected: testDate2,
                actual: note.LastUpdatedAt);
        }

        [Fact_a]
        public void CanSetUpdatedAtToLaterDateTime()
        {
            System.DateTime testDate = new(
                year: 2026, month: 7, day: 1,
                hour: 13, minute: 34, second: 56,
                millisecond: 789);

            System.DateTime testDate2 = new(
                year: 2026, month: 7, day: 1,
                hour: 15, minute: 34, second: 56,
                millisecond: 789);

            Note note = new();
            note.LastUpdatedAt = testDate;

            Assert.Equal(expected: testDate,
                actual: note.LastUpdatedAt);

            note.LastUpdatedAt = testDate2;

            Assert.Equal(expected: testDate2,
                actual: note.LastUpdatedAt);
        }

        [Fact_a]
        public void CannotSetUpdatedAtToEarlierDateTime()
        {
            System.DateTime testDate = new(
                year: 2026, month: 7, day: 1,
                hour: 13, minute: 34, second: 56,
                millisecond: 789);

            System.DateTime testDate2 = new(
                year: 2026, month: 7, day: 1,
                hour: 11, minute: 34, second: 56,
                millisecond: 789);

            Note note = new();
            note.LastUpdatedAt = testDate;
            Assert.Equal(expected: testDate, actual: note.LastUpdatedAt);

            Assert.Throws<NoteLastModifiedDateCannotGoBackInTimeException>(
                () => note.LastUpdatedAt = testDate2);

            Assert.Equal(expected: testDate,
                actual: note.LastUpdatedAt);
        }

        [Fact_a]
        public void VerifyFirstParagraphLength()
        {
            Assert_t.Equal(expected: 695,
                actual: TheHoundOfTheBaskervilles.TheLegend.First.Length);
        }

        [Fact_a]
        public void VerifySecondParagraphLength()
        {
            Assert_t.Equal(expected: 1739,
                actual: TheHoundOfTheBaskervilles.TheLegend.Second.Length);
        }

        [Fact_a]
        public void VerifyThirdParagraphLength()
        {
            Assert_t.Equal(expected: 934,
                actual: TheHoundOfTheBaskervilles.TheLegend.Third.Length);
        }

        [Fact_a]
        public void VerifyFourthParagraphLength()
        {
            Assert_t.Equal(expected: 628,
                actual: TheHoundOfTheBaskervilles.TheLegend.Fourth.Length);
        }

        [Fact_a]
        public void VerifyFifthParagraphLength()
        {
            Assert_t.Equal(expected: 1278,
                actual: TheHoundOfTheBaskervilles.TheLegend.Fifth.Length);
        }

        [Fact_a]
        public void VerifySixthParagraphLength()
        {
            Assert_t.Equal(expected: 1270,
                actual: TheHoundOfTheBaskervilles.TheLegend.Sixth.Length);
        }

        [Fact_a]
        public void VerifySeventhParagraphLength()
        {
            Assert_t.Equal(expected: 737,
                actual: TheHoundOfTheBaskervilles.TheLegend.Seventh.Length);
        }

        [Fact_a]
        public void VerifyPostScriptLength()
        {
            Assert_t.Equal(expected: 134,
                actual: TheHoundOfTheBaskervilles.TheLegend.PostScript.Length);
        }
    }
}

namespace NullPointersEtc.NotesJournalApp.TheHoundOfTheBaskervilles
{
    public class TheLegend
    {
        public static string First
        {
            get => "Of the origin of the Hound of the Baskervilles " +
                "there have been many statements, yet as I come in " +
                "a direct line from Hugo Baskerville, and as I had " +
                "the story from my father, who also had it from " +
                "his, I have set it down with all belief that it " +
                "occurred even as is here set forth. And I would " +
                "have you believe, my sons, that the same Justice " +
                "which punishes sin may also most graciously " +
                "forgive it, and that no ban is so heavy but that " +
                "by prayer and repentance it may be removed. Learn " +
                "then from this story not to fear the fruits of the " +
                "past, but rather to be circumspect in the future, " +
                "that those foul passions whereby our family has " +
                "suffered so grievously may not again be loosed to " +
                "our undoing.\r\n";
        }

        public static string Second
        {
            get => "Know then that in the time of the Great " +
                "Rebellion (the history of which by the learned " +
                "Lord Clarendon I most earnestly commend to your " +
                "attention) this Manor of Baskerville was held by " +
                "Hugo of that name, nor can it be gainsaid that he " +
                "was a most wild, profane, and godless man. This, " +
                "in truth, his neighbours might have pardoned, " +
                "seeing that saints have never flourished in those " +
                "parts, but there was in him a certain wanton and " +
                "cruel humour which made his name a byword through " +
                "the West. It chanced that this Hugo came to love " +
                "(if, indeed, so dark a passion may be known under " +
                "so bright a name) the daughter of a yeoman who " +
                "held lands near the Baskerville estate. But the " +
                "young maiden, being discreet and of good repute, " +
                "would ever avoid him, for she feared his evil " +
                "name. So it came to pass that one Michaelmas this " +
                "Hugo, with five or six of his idle and wicked " +
                "companions, stole down upon the farm and carried " +
                "off the maiden, her father and brothers being from " +
                "home, as he well knew. When they had brought her " +
                "to the Hall the maiden was placed in an upper " +
                "chamber, while Hugo and his friends sat down to a " +
                "long carouse, as was their nightly custom. Now, " +
                "the poor lass upstairs was like to have her wits " +
                "turned at the singing and shouting and terrible " +
                "oaths which came up to her from below, for they " +
                "say that the words used by Hugo Baskerville, when " +
                "he was in wine, were such as might blast the man " +
                "who said them. At last in the stress of her fear " +
                "she did that which might have daunted the bravest " +
                "or most active man, for by the aid of the growth " +
                "of ivy which covered (and still covers) the south " +
                "wall she came down from under the eaves, and so " +
                "homeward across the moor, there being three " +
                "leagues betwixt the Hall and her father's farm.\r\n";
        }

        public static string Third
        {
            get => "It chanced that some little time later Hugo " +
                "left his guests to carry food and drink\u2014" +
                "with other worse things, perchance\u2014" +
                "to his captive, and so " +
                "found the cage empty and the bird escaped. Then, " +
                "as it would seem, he became as one that hath a " +
                "devil, for, rushing down the stairs into the " +
                "dining-hall, he sprang upon the great table, " +
                "flagons and trenchers flying before him, and he " +
                "cried aloud before all the company that he would " +
                "that very night render his body and soul to the " +
                "Powers of Evil if he might but overtake the wench. " +
                "And while the revellers stood aghast at the fury " +
                "of the man, one more wicked or, it may be, more " +
                "drunken than the rest, cried out that they should " +
                "put the hounds upon her. Whereat Hugo ran from the " +
                "house, crying to his grooms that they should " +
                "saddle his mare and unkennel the pack, and giving " +
                "the hounds a kerchief of the maid's, he swung them " +
                "to the line, and so off full cry in the moonlight " +
                "over the moor.\r\n";
        }

        public static string Fourth
        {
            get => "Now, for some space the revellers stood agape, " +
                "unable to understand all that had been done in " +
                "such haste. But anon their bemused wits awoke to " +
                "the nature of the deed which was like to be done " +
                "upon the moorlands. Everything was now in an " +
                "uproar, some calling for their pistols, some for " +
                "their horses, and some for another flask of wine. " +
                "But at length some sense came back to their crazed " +
                "minds, and the whole of them, thirteen in number, " +
                "took horse and started in pursuit. The moon shone " +
                "clear above them, and they rode swiftly abreast, " +
                "taking that course which the maid must needs have " +
                "taken if she were to reach her own home.\r\n";
        }

        public static string Fifth
        {
            get => "They had gone a mile or two when they passed " +
                "one of the night shepherds upon the moorlands, and " +
                "they cried to him to know if he had seen the hunt. " +
                "And the man, as the story goes, was so crazed with " +
                "fear that he could scarce speak, but at last he " +
                "said that he had indeed seen the unhappy maiden, " +
                "with the hounds upon her track. 'But I have seen " +
                "more than that,' said he, 'for Hugo Baskerville " +
                "passed me upon his black mare, and there ran mute " +
                "behind him such a hound of hell as God forbid " +
                "should ever be at my heels.' So the drunken " +
                "squires cursed the shepherd and rode onward. But " +
                "soon their skins turned cold, for there came a " +
                "galloping across the moor, and the black mare, " +
                "dabbled with white froth, went past with trailing " +
                "bridle and empty saddle. Then the revellers rode " +
                "close together, for a great fear was on them, but " +
                "they still followed over the moor, though each, " +
                "had he been alone, would have been right glad to " +
                "have turned his horse's head. Riding slowly in " +
                "this fashion they came at last upon the hounds. " +
                "These, though known for their valour and their " +
                "breed, were whimpering in a cluster at the head of " +
                "a deep dip or goyal, as we call it, upon the moor, " +
                "some slinking away and some, with starting hackles " +
                "and staring eyes, gazing down the narrow valley " +
                "before them.\r\n";
        }

        public static string Sixth
        {
            get => "The company had come to a halt, more sober men, " +
                "as you may guess, than when they started. The most " +
                "of them would by no means advance, but three of " +
                "them, the boldest, or it may be the most drunken, " +
                "rode forward down the goyal. Now, it opened into a " +
                "broad space in which stood two of those great " +
                "stones, still to be seen there, which were set by " +
                "certain forgotten peoples in the days of old. The " +
                "moon was shining bright upon the clearing, and " +
                "there in the centre lay the unhappy maid where she " +
                "had fallen, dead of fear and of fatigue. But it " +
                "was not the sight of her body, nor yet was it that " +
                "of the body of Hugo Baskerville lying near her, " +
                "which raised the hair upon the heads of these " +
                "three daredevil roysterers, but it was that, " +
                "standing over Hugo, and plucking at his throat, " +
                "there stood a foul thing, a great, black beast, " +
                "shaped like a hound, yet larger than any hound " +
                "that ever mortal eye has rested upon. And even as " +
                "they looked the thing tore the throat out of Hugo " +
                "Baskerville, on which, as it turned its blazing " +
                "eyes and dripping jaws upon them, the three " +
                "shrieked with fear and rode for dear life, still " +
                "screaming, across the moor. One, it is said, died " +
                "that very night of what he had seen, and the other " +
                "twain were but broken men for the rest of their " +
                "days.\r\n";
        }

        public static string Seventh
        {
            get => "Such is the tale, my sons, of the coming of the " +
                "hound which is said to have plagued the family so " +
                "sorely ever since. If I have set it down it is " +
                "because that which is clearly known hath less " +
                "terror than that which is but hinted at and " +
                "guessed. Nor can it be denied that many of the " +
                "family have been unhappy in their deaths, which " +
                "have been sudden, bloody, and mysterious. Yet may " +
                "we shelter ourselves in the infinite goodness of " +
                "Providence, which would not forever punish the " +
                "innocent beyond that third or fourth generation " +
                "which is threatened in Holy Writ. To that " +
                "Providence, my sons, I hereby commend you, and I " +
                "counsel you by way of caution to forbear from " +
                "crossing the moor in those dark hours when the " +
                "powers of evil are exalted.\r\n";
        }

        public static string PostScript
        {
            get => "[This from Hugo Baskerville to his sons Rodger " +
                "and John, with instructions that they say nothing " +
                "thereof to their sister Elizabeth.]\r\n";
        }
    }
}
#endregion "NoteEntityTests.cs"
