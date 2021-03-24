using System;
using System.Text.RegularExpressions;

namespace CreatureScriptsParser
{
    public static class LineGetters
    {
        public static string GetGuidFromLine(string line, bool unitGuid = false, bool senderGuid = false, bool moverGuid = false, bool attackerGuid = false, bool casterGuid = false, bool hitTargetGuid = false, bool castGuid = false, bool oneShotAnimKitGuid = false, bool emoteGuid = false, bool conversationActorGuid = false)
        {
            if (!line.Contains("TypeName: Creature; Full:") && !line.Contains("TypeName: Vehicle; Full:") &&
                !line.Contains("TypeName: Cast; Full:") && !line.Contains("TypeName: Conversation; Full:") && !casterGuid)
                return "";

            Regex objectTypeRegex = new Regex(@"[a-zA-Z]+;{1}\s{1}Full:{1}\s");

            if (unitGuid)
            {
                Regex guidRegex = new Regex(@"UnitGUID: TypeName:{1}\s{1}[a-zA-Z]+;{1}\s{1}Full:{1}\s{1}\w{20,}");
                if (guidRegex.IsMatch(line))
                    return guidRegex.Match(line).ToString().Replace("UnitGUID: TypeName: ", "").Replace(objectTypeRegex.Match(line).ToString(), "");
            }
            else if (senderGuid)
            {
                Regex guidRegex = new Regex(@"SenderGUID: TypeName:{1}\s{1}[a-zA-Z]+;{1}\s{1}Full:{1}\s{1}\w{20,}");
                if (guidRegex.IsMatch(line))
                    return guidRegex.Match(line).ToString().Replace("SenderGUID: TypeName: ", "").Replace(objectTypeRegex.Match(line).ToString(), "");
            }
            else if (moverGuid)
            {
                Regex guidRegex = new Regex(@"MoverGUID: TypeName:{1}\s{1}[a-zA-Z]+;{1}\s{1}Full:{1}\s{1}\w{20,}");
                if (guidRegex.IsMatch(line))
                    return guidRegex.Match(line).ToString().Replace("MoverGUID: TypeName: ", "").Replace(objectTypeRegex.Match(line).ToString(), "");
            }
            else if (attackerGuid)
            {
                Regex guidRegex = new Regex(@"Attacker Guid: TypeName:{1}\s{1}[a-zA-Z]+;{1}\s{1}Full:{1}\s{1}\w{20,}");
                if (guidRegex.IsMatch(line))
                    return guidRegex.Match(line).ToString().Replace("Attacker Guid: TypeName: ", "").Replace(objectTypeRegex.Match(line).ToString(), "");
            }
            else if (casterGuid)
            {
                Regex guidRegex = new Regex(@"CasterGUID: TypeName:{1}\s{1}[a-zA-Z]+;{1}\s{1}Full:{1}\s{1}\w{20,}");
                if (guidRegex.IsMatch(line))
                    return guidRegex.Match(line).ToString().Replace("CasterGUID: TypeName: ", "").Replace(objectTypeRegex.Match(line).ToString(), "");
            }
            else if (hitTargetGuid)
            {
                Regex guidRegex = new Regex(@"HitTarget: TypeName:{1}\s{1}[a-zA-Z]+;{1}\s{1}Full:{1}\s{1}\w{20,}");
                if (guidRegex.IsMatch(line))
                    return guidRegex.Match(line).ToString().Replace("HitTarget: TypeName: ", "").Replace(objectTypeRegex.Match(line).ToString(), "");
            }
            else if (castGuid)
            {
                Regex guidRegex = new Regex(@"CastID: TypeName:{1}\s{1}[a-zA-Z]+;{1}\s{1}Full:{1}\s{1}\w{20,}");
                if (guidRegex.IsMatch(line))
                    return guidRegex.Match(line).ToString().Replace("CastID: TypeName: ", "").Replace(objectTypeRegex.Match(line).ToString(), "");
            }
            else if (oneShotAnimKitGuid)
            {
                Regex guidRegex = new Regex(@"Unit: TypeName:{1}\s{1}[a-zA-Z]+;{1}\s{1}Full:{1}\s{1}\w{20,}");
                if (guidRegex.IsMatch(line))
                    return guidRegex.Match(line).ToString().Replace("Unit: TypeName: ", "").Replace(objectTypeRegex.Match(line).ToString(), "");
            }
            else if (emoteGuid)
            {
                Regex guidRegex = new Regex(@"GUID: TypeName:{1}\s{1}[a-zA-Z]+;{1}\s{1}Full:{1}\s{1}\w{20,}");
                if (guidRegex.IsMatch(line))
                    return guidRegex.Match(line).ToString().Replace("GUID: TypeName: ", "").Replace(objectTypeRegex.Match(line).ToString(), "");
            }
            else if (conversationActorGuid)
            {
                Regex guidRegex = new Regex(@"ActorGUID: TypeName:{1}\s{1}[a-zA-Z]+;{1}\s{1}Full:{1}\s{1}\w{20,}");
                if (guidRegex.IsMatch(line))
                    return guidRegex.Match(line).ToString().Replace("ActorGUID: TypeName: ", "").Replace(objectTypeRegex.Match(line).ToString(), "");
            }
            else
            {
                Regex guidRegexFirst = new Regex(@"ObjectGuid: TypeName:{1}\s{1}[a-zA-Z]+;{1}\s{1}Full:{1}\s{1}\w{20,}");
                if (guidRegexFirst.IsMatch(line))
                    return guidRegexFirst.Match(line).ToString().Replace("ObjectGuid: TypeName: ", "").Replace(objectTypeRegex.Match(line).ToString(), "");

                Regex guidRegexSecond = new Regex(@"ObjectGUID: TypeName:{1}\s{1}[a-zA-Z]+;{1}\s{1}Full:{1}\s{1}\w{20,}");
                if (guidRegexSecond.IsMatch(line))
                    return guidRegexSecond.Match(line).ToString().Replace("ObjectGUID: TypeName: ", "").Replace(objectTypeRegex.Match(line).ToString(), "");
            }

            return "";
        }

        public static TimeSpan GetTimeSpanFromLine(string timeSpanLine)
        {
            int days = 0;
            int hours = 0;
            int minutes = 0;
            int seconds = 0;
            int milliseconds = 0;

            Regex dateRegex = new Regex(@"Time:{1}\s+\d+/+\d+/+\d+");
            Regex timeRegex = new Regex(@"\d+:+\d+:+.+\s{1}Number");

            if (dateRegex.IsMatch(timeSpanLine))
            {
                string[] date = dateRegex.Match(timeSpanLine).ToString().Replace("Time: ", "").Split('/');

                days = Convert.ToInt32(date[1]);
            }

            if (timeRegex.IsMatch(timeSpanLine))
            {
                string[] time = timeRegex.Match(timeSpanLine).ToString().Replace(" Number", "").Split(':');

                hours = Convert.ToInt32(time[0]);
                minutes = Convert.ToInt32(time[1]);

                string tempTime = time[2];
                string[] splittedTime = tempTime.Split('.');

                seconds = Convert.ToInt32(splittedTime[0]);
                milliseconds = Convert.ToInt32(splittedTime[1]);
            }

            return new TimeSpan(days, hours, minutes, seconds, milliseconds);
        }

        public static bool IsCreatureLine(this string value)
        {
            if ((value.Contains("Creature") || value.Contains("Vehicle")) &&
                (value.Contains("ObjectGuid:") || value.Contains("SenderGUID:") ||
                value.Contains("MoverGUID:") || value.Contains("Attacker Guid:") ||
                value.Contains("ObjectGUID:") || value.Contains("Unit: TypeName:")))
                return true;

            return false;
        }

        public static bool IsConversationLine(this string value)
        {
            if (value.Contains("ObjectGuid: TypeName: Conversation; Full:"))
                return true;

            return false;
        }

        public static long GetPacketNumberFromLine(string line)
        {
            Regex numberRegex = new Regex(@"Number:{1}\s{1}\w+");
            if (numberRegex.IsMatch(line))
                return Convert.ToInt64(numberRegex.Match(line).ToString().Replace("Number: ", ""));

            return 0;
        }
    }
}
