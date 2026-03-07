using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using CEOGame.Data;

namespace CEOGame.Editor
{
    public static class AssetGenerator
    {
        const string EmployeePath = "Assets/ScriptableObjects/Employees/";
        const string RequestPath = "Assets/ScriptableObjects/Requests/";

        [MenuItem("Tools/CEO Game/Generate All Assets")]
        public static void GenerateAll()
        {
            ClearFolder(EmployeePath);
            ClearFolder(RequestPath);
            AssetDatabase.Refresh();

            var employees = CreateAllEmployees();
            var requests = CreateAllRequests(employees);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"Generated {employees.Count} employees and {requests.Count} requests.");
        }

        static void ClearFolder(string folder)
        {
            string fullPath = Path.Combine(Application.dataPath, "..", folder);
            if (Directory.Exists(fullPath))
            {
                foreach (var file in Directory.GetFiles(fullPath, "*.asset"))
                    File.Delete(file);
                foreach (var file in Directory.GetFiles(fullPath, "*.asset.meta"))
                    File.Delete(file);
            }
            else
            {
                Directory.CreateDirectory(fullPath);
            }
        }

        static EmployeeData CreateEmployee(string assetName, string displayName, Team team, Position position, int salary, int happiness, string bio = "", string hrTip = "")
        {
            var e = ScriptableObject.CreateInstance<EmployeeData>();
            e.employeeName = displayName;
            e.team = team;
            e.position = position;
            e.salary = salary;
            e.happiness = happiness;
            e.personalityBio = bio;
            e.hrTip = hrTip;
            e.goodRelationships = new EmployeeData[0];
            e.neutralRelationships = new EmployeeData[0];
            e.badRelationships = new EmployeeData[0];
            AssetDatabase.CreateAsset(e, EmployeePath + assetName + ".asset");
            return e;
        }

        static RequestData CreateRequest(string assetName, EmployeeData employee, RequestCategory category, string dialogue,
            DecisionOutcome approve, DecisionOutcome deny,
            RequestData[] requiresApproved = null, RequestData[] requiresDenied = null)
        {
            var r = ScriptableObject.CreateInstance<RequestData>();
            r.requestingEmployee = employee;
            r.category = category;
            r.dialogueLines = dialogue.Split(new[] { "\n\n" }, System.StringSplitOptions.RemoveEmptyEntries);
            r.approveOutcome = approve;
            r.denyOutcome = deny;
            r.requiresApproved = requiresApproved ?? new RequestData[0];
            r.requiresDenied = requiresDenied ?? new RequestData[0];
            AssetDatabase.CreateAsset(r, RequestPath + assetName + ".asset");
            return r;
        }

        static Dictionary<string, EmployeeData> CreateAllEmployees()
        {
            var d = new Dictionary<string, EmployeeData>();

            // Key employees with full data
            d["Ralitsa"] = CreateEmployee("Ralitsa", "\u0420\u0430\u043b\u0438\u0446\u0430", Team.HR, Position.Lead, 3000, 75,
                "\u0414\u044a\u0440\u0436\u0438 \u0444\u0438\u0440\u043c\u0430\u0442\u0430 \u0434\u0430 \u043d\u0435 \u0441\u0435 \u0440\u0430\u0437\u043f\u0430\u0434\u043d\u0435. \u0413\u043e\u0432\u043e\u0440\u0438 \u0431\u044a\u0440\u0437\u043e, \u043d\u043e \u044f\u0441\u043d\u043e.", "(tutorial)");

            d["Ginka"] = CreateEmployee("Ginka", "\u0413\u0438\u043d\u043a\u0430", Team.Janitor, Position.Mid, 1500, 70,
                "\u0413\u0438\u043d\u043a\u0430 \u0435 \u0432\u044a\u0432 \u0444\u0438\u0440\u043c\u0430\u0442\u0430 \u043e\u0442 20 \u0433\u043e\u0434\u0438\u043d\u0438. \u041f\u0430\u0437\u0438 \u0440\u0435\u0434\u0430 \u0438 \u0445\u043e\u0440\u0430\u0442\u0430 \u044f \u0443\u0432\u0430\u0436\u0430\u0432\u0430\u0442.",
                "\u0410\u043a\u043e \u043e\u0442\u043a\u0430\u0436\u0435\u0442\u0435 \u0441\u043c\u044f\u043d\u0430 \u043d\u0430 \u043f\u043e\u0437\u0438\u0446\u0438\u044f\u0442\u0430, \u0442\u044f \u0449\u0435 \u043d\u0430\u043f\u0443\u0441\u043d\u0435. \u041c\u043e\u0440\u0430\u043b\u044a\u0442 \u0449\u0435 \u043f\u0430\u0434\u043d\u0435 \u0441 \u043c\u043d\u043e\u0433\u043e.");

            d["Kalin"] = CreateEmployee("Kalin", "\u041a\u0430\u043b\u0438\u043d", Team.Programming, Position.Mid, 3000, 55,
                "\u0421\u0430\u043c\u043e\u0443\u0432\u0435\u0440\u0435\u043d. \u0427\u0435\u0441\u0442\u043e \u0438\u0434\u0432\u0430 '\u0441\u043b\u0435\u0434 \u0442\u0435\u0436\u043a\u0430 \u0432\u0435\u0447\u0435\u0440'.",
                "\u041f\u043e\u0432\u0435\u0447\u0435\u0442\u043e \u0445\u043e\u0440\u0430 \u043d\u0435 \u0433\u043e \u043f\u043e\u043d\u0430\u0441\u044f\u0442. \u0418\u043c\u0430 \u043f\u0440\u043e\u0431\u043b\u0435\u043c\u0438 \u0441 \u0430\u043b\u043a\u043e\u0445\u043e\u043b\u0430 \u0438 \u0447\u0435\u0441\u0442\u043e \u0441\u044a\u0437\u0434\u0430\u0432\u0430 \u043a\u043e\u043d\u0444\u043b\u0438\u043a\u0442\u0438.");

            d["Marin"] = CreateEmployee("Marin", "\u041c\u0430\u0440\u0438\u043d", Team.Management, Position.Director, 5000, 95,
                "\u0420\u0430\u0446\u0438\u043e\u043d\u0430\u043b\u0435\u043d, \u0433\u043e\u0432\u043e\u0440\u0438 \u0441 \u0446\u0438\u0444\u0440\u0438. \u041e\u0431\u0438\u0447\u0430 '\u043f\u043b\u0430\u043d\u043e\u0432\u0435' \u0438 \u0432\u0435\u0440\u043e\u044f\u0442\u043d\u043e\u0441\u0442\u0438.",
                "\u041c\u0430\u0440\u0438\u043d \u043d\u0435 \u0431\u043b\u044a\u0444\u0438\u0440\u0430. \u0418\u043c\u0430 \u0440\u0435\u0430\u043b\u043d\u0438 \u043a\u043e\u043d\u0442\u0430\u043a\u0442\u0438 \u0441 \u0438\u043d\u0432\u0435\u0441\u0442\u0438\u0442\u043e\u0440\u0438. \u041d\u043e '80/20' \u0435 \u043f\u043e-\u0441\u043a\u043e\u0440\u043e \u0443\u0432\u0435\u0440\u0435\u043d\u043e\u0441\u0442, \u043d\u0435 \u0433\u0430\u0440\u0430\u043d\u0446\u0438\u044f.");

            d["Evgeniy"] = CreateEmployee("Evgeniy", "\u0415\u0432\u0433\u0435\u043d\u0438\u0439", Team.GameDesign, Position.Mid, 3000, 90,
                "\u0415\u043d\u0435\u0440\u0433\u0438\u0447\u0435\u043d, \u0441\u043e\u0446\u0438\u0430\u043b\u0435\u043d. \u0412\u044f\u0440\u0432\u0430 \u0432 \u0445\u043e\u0440\u0430\u0442\u0430 \u0438 \u0432 \u0435\u043a\u0438\u043f\u043d\u0430\u0442\u0430 \u0445\u0438\u043c\u0438\u044f.",
                "\u0415\u0432\u0433\u0435\u043d\u0438\u0439 \u043f\u0440\u043e\u0441\u0442\u043e \u0441\u0435 \u043e\u043f\u0438\u0442\u0432\u0430 \u0434\u0430 \u0443\u0440\u0435\u0434\u0438 \u0440\u0430\u0431\u043e\u0442\u0430 \u043d\u0430 \u0441\u0432\u043e\u0439 \u043f\u0440\u0438\u044f\u0442\u0435\u043b. \u041c\u0438\u0441\u043b\u0438, \u0447\u0435 \u0449\u0435 \u043f\u043e\u043c\u043e\u0433\u043d\u0435 \u043d\u0430 \u0435\u043a\u0438\u043f\u0430.");

            d["Denislava"] = CreateEmployee("Denislava", "\u0414\u0435\u043d\u0438\u0441\u043b\u0430\u0432\u0430", Team.Finance, Position.Senior, 3000, 80,
                "\u041f\u043e\u0434\u0440\u0435\u0434\u0435\u043d\u0430 \u0438 \u0434\u0438\u0440\u0435\u043a\u0442\u043d\u0430. \u0412\u0438\u0436\u0434\u0430 \u0444\u0438\u0440\u043c\u0430\u0442\u0430 \u043a\u0430\u0442\u043e \u0447\u0438\u0441\u043b\u0430 \u0438 \u0440\u0438\u0441\u043a.",
                "\u0414\u0435\u043d\u0438\u0441\u043b\u0430\u0432\u0430 \u043d\u0430\u0438\u0441\u0442\u0438\u043d\u0430 \u0441\u0435 \u043e\u043f\u0438\u0442\u0432\u0430 \u0434\u0430 \u043f\u0440\u0435\u0434\u043e\u0442\u0432\u0440\u0430\u0442\u0438 \u0444\u0438\u043d\u0430\u043d\u0441\u043e\u0432 \u0441\u0440\u0438\u0432.");

            d["Trifon"] = CreateEmployee("Trifon", "\u0422\u0440\u0438\u0444\u043e\u043d", Team.Art, Position.Senior, 5000, 65,
                "\u041f\u0435\u0440\u0444\u0435\u043a\u0446\u0438\u043e\u043d\u0438\u0441\u0442. \u041f\u0430\u0437\u0438 \u043a\u0430\u0447\u0435\u0441\u0442\u0432\u043e\u0442\u043e, \u043d\u043e \u0432\u0435\u0447\u0435 \u0435 \u043d\u0430 \u0440\u044a\u0431\u0430.",
                "\u0422\u0440\u0438\u0444\u043e\u043d \u043d\u0435 \u0438\u0441\u043a\u0430 \u043f\u043e\u0432\u0435\u0447\u0435 \u043f\u0430\u0440\u0438. \u0418\u0441\u043a\u0430 \u043f\u043e-\u043c\u0430\u043b\u043a\u043e \u043e\u0431\u0435\u043c. \u0410\u043a\u043e \u0433\u043e \u043e\u0442\u0440\u0435\u0436\u0435\u0448, \u0440\u0438\u0441\u043a\u044a\u0442 \u043e\u0442 \u043f\u0440\u0435\u0433\u0430\u0440\u044f\u043d\u0435 \u0438 \u043d\u0430\u043f\u0443\u0441\u043a\u0430\u043d\u0435 \u0440\u0430\u0441\u0442\u0435.");

            d["Debora"] = CreateEmployee("Debora", "\u0414\u0435\u0431\u043e\u0440\u0430", Team.QA, Position.Lead, 5000, 44,
                "\u041e\u0440\u0433\u0430\u043d\u0438\u0437\u0438\u0440\u0430 \u0445\u0430\u043e\u0441\u0430. \u0412\u0438\u0436\u0434\u0430 \u0440\u0438\u0441\u043a\u043e\u0432\u0435\u0442\u0435 \u043f\u0440\u0435\u0434\u0438 \u0432\u0441\u0438\u0447\u043a\u0438.",
                "\u0414\u0435\u0431\u043e\u0440\u0430 \u043d\u0435 \u0442\u044a\u0440\u0441\u0438 \u0434\u0440\u0430\u043c\u0430. QA \u0435 \u043d\u0430 \u0440\u044a\u0431\u0430 \u0438 \u0431\u0435\u0437 \u043e\u0449\u0435 \u0445\u043e\u0440\u0430 \u0449\u0435 \u0437\u0430\u043f\u043e\u0447\u043d\u0430\u0442 \u0434\u0430 \u0438\u0437\u043f\u0443\u0441\u043a\u0430\u0442 \u043a\u0440\u0438\u0442\u0438\u0447\u043d\u0438 \u043f\u0440\u043e\u0431\u043b\u0435\u043c\u0438.");

            // Other employees - no bio/hrTip
            d["Vesko"] = CreateEmployee("Vesko", "\u0412\u0435\u0441\u043a\u043e", Team.Management, Position.Lead, 5000, 75);
            d["Sashka"] = CreateEmployee("Sashka", "\u0421\u0430\u0448\u043a\u0430", Team.Management, Position.Senior, 5000, 70);
            d["Valya"] = CreateEmployee("Valya", "\u0412\u0430\u043b\u044f", Team.Programming, Position.Senior, 3000, 75);
            d["Dimitrina"] = CreateEmployee("Dimitrina", "\u0414\u0438\u043c\u0438\u0442\u0440\u0438\u043d\u0430", Team.Programming, Position.Mid, 3000, 70);
            d["Tanya"] = CreateEmployee("Tanya", "\u0422\u0430\u043d\u044f", Team.Programming, Position.Mid, 3000, 70);
            d["Ahil"] = CreateEmployee("Ahil", "\u0410\u0445\u0438\u043b", Team.Programming, Position.Junior, 1500, 65);
            d["Spartak"] = CreateEmployee("Spartak", "\u0421\u043f\u0430\u0440\u0442\u0430\u043a", Team.Art, Position.Lead, 5000, 70);
            d["Pavlin"] = CreateEmployee("Pavlin", "\u041f\u0430\u0432\u043b\u0438\u043d", Team.Art, Position.Mid, 3000, 75);
            d["Irina"] = CreateEmployee("Irina", "\u0418\u0440\u0438\u043d\u0430", Team.Art, Position.Mid, 3000, 70);
            d["Petranka"] = CreateEmployee("Petranka", "\u041f\u0435\u0442\u0440\u0430\u043d\u043a\u0430", Team.Art, Position.Junior, 1500, 70);
            d["Vinsant"] = CreateEmployee("Vinsant", "\u0412\u0438\u043d\u0441\u044a\u043d\u0442", Team.Art, Position.Senior, 3000, 65);
            d["Zhan"] = CreateEmployee("Zhan", "\u0416\u0430\u043d", Team.GameDesign, Position.Lead, 5000, 75);
            d["Rada"] = CreateEmployee("Rada", "\u0420\u0430\u0434\u0430", Team.GameDesign, Position.Junior, 1500, 80);
            d["Plamen"] = CreateEmployee("Plamen", "\u041f\u043b\u0430\u043c\u0435\u043d", Team.QA, Position.Senior, 3000, 60);
            d["Nikol"] = CreateEmployee("Nikol", "\u041d\u0438\u043a\u043e\u043b", Team.QA, Position.Mid, 3000, 65);
            d["Haralambi"] = CreateEmployee("Haralambi", "\u0425\u0430\u0440\u0430\u043b\u0430\u043c\u0431\u0438", Team.Marketing, Position.Lead, 5000, 70);
            d["Petya"] = CreateEmployee("Petya", "\u041f\u0435\u0442\u044f", Team.Marketing, Position.Mid, 3000, 75);
            d["Dinko"] = CreateEmployee("Dinko", "\u0414\u0438\u043d\u043a\u043e", Team.CustomerSupport, Position.Senior, 3000, 60);
            d["Milen"] = CreateEmployee("Milen", "\u041c\u0438\u043b\u0435\u043d", Team.CustomerSupport, Position.Mid, 1500, 65);
            d["Iva"] = CreateEmployee("Iva", "\u0418\u0432\u0430", Team.CustomerSupport, Position.Junior, 1500, 70);
            d["Mincho"] = CreateEmployee("Mincho", "\u041c\u0438\u043d\u0447\u043e", Team.Janitor, Position.Mid, 1500, 60);
            d["Tsvetelin"] = CreateEmployee("Tsvetelin", "\u0426\u0432\u0435\u0442\u0435\u043b\u0438\u043d", Team.CEO, Position.Director, 5000, 80);

            // Wire relationships for key employees
            WireRelationships(d);

            return d;
        }

        static void WireRelationships(Dictionary<string, EmployeeData> d)
        {
            // Ginka: good with most, bad with Dinko (CustomerSupport senior)
            d["Ginka"].goodRelationships = new[] { d["Ralitsa"], d["Mincho"], d["Spartak"], d["Pavlin"] };
            d["Ginka"].neutralRelationships = new[] { d["Milen"], d["Iva"] };
            d["Ginka"].badRelationships = new[] { d["Dinko"] };

            // Kalin: bad with most programmers
            d["Kalin"].badRelationships = new[] { d["Valya"], d["Dimitrina"], d["Tanya"], d["Ahil"] };
            d["Kalin"].neutralRelationships = new[] { d["Marin"] };

            // Marin: good with management
            d["Marin"].goodRelationships = new[] { d["Vesko"], d["Sashka"], d["Denislava"] };
            d["Marin"].neutralRelationships = new[] { d["Tsvetelin"] };

            // Evgeniy: good with design team
            d["Evgeniy"].goodRelationships = new[] { d["Zhan"], d["Rada"] };

            // Denislava: good with Marin, neutral with most
            d["Denislava"].goodRelationships = new[] { d["Marin"] };

            // Trifon: good with art team
            d["Trifon"].goodRelationships = new[] { d["Spartak"], d["Pavlin"], d["Irina"] };
            d["Trifon"].neutralRelationships = new[] { d["Vinsant"], d["Petranka"] };

            // Debora: good with QA team
            d["Debora"].goodRelationships = new[] { d["Plamen"], d["Nikol"] };

            // Mark dirty so Unity saves the relationship changes
            foreach (var kvp in d)
                EditorUtility.SetDirty(kvp.Value);
        }

        static List<RequestData> CreateAllRequests(Dictionary<string, EmployeeData> emp)
        {
            var requests = new List<RequestData>();

            // 1. Ginka - TeamTransfer
            requests.Add(CreateRequest("GinkaRequest", emp["Ginka"], RequestCategory.TeamTransfer,
                "\u0414\u043e\u0431\u044a\u0440 \u0434\u0435\u043d, \u0448\u0435\u0444\u0435. \u041d\u044f\u043c\u0430 \u0434\u0430 \u0432\u0438 \u0437\u0430\u043d\u0438\u043c\u0430\u0432\u0430\u043c \u043c\u043d\u043e\u0433\u043e.\n\n\u0414\u0432\u0430\u0434\u0435\u0441\u0435\u0442 \u0433\u043e\u0434\u0438\u043d\u0438 \u0441\u044a\u043c \u0442\u0443\u043a\u2026 \u0430\u043c\u0430 \u043a\u0440\u044a\u0441\u0442\u044a\u0442 \u0432\u0435\u0447\u0435 \u043d\u0435 \u0434\u044a\u0440\u0436\u0438. \u041a\u043e\u0433\u0430\u0442\u043e \u0447\u0438\u0441\u0442\u044f, \u043c\u0435 \u0441\u0440\u044f\u0437\u0432\u0430 \u0432 \u043a\u0440\u044a\u0441\u0442\u0430 \u0438 \u043f\u043e\u0441\u043b\u0435 \u043d\u0435 \u043c\u043e\u0433\u0430 \u0434\u0430 \u0441\u0435 \u0438\u0437\u043f\u0440\u0430\u0432\u044f.\n\n\u0418\u0441\u043a\u0430\u043c \u0434\u0430 \u043c\u0435 \u043f\u0440\u0435\u043c\u0435\u0441\u0442\u0438\u0442\u0435 \u0432 'Customer Support'. \u041d\u044f\u043c\u0430 \u0434\u0430 \u0432\u0438 \u0438\u0441\u043a\u0430\u043c \u043f\u043e\u0432\u0435\u0447\u0435 \u043f\u0430\u0440\u0438, \u0441\u0430\u043c\u043e \u0434\u0430 \u043c\u043e\u0433\u0430 \u0434\u0430 \u0440\u0430\u0431\u043e\u0442\u044f, \u0431\u0435\u0437 \u0434\u0430 \u043c\u0435 \u0431\u043e\u043b\u0438.",
                new DecisionOutcome { budgetChange = 0, moraleChange = 6, peopleChange = 0,
                    outcomeText = "\u0411\u043b\u0430\u0433\u043e\u0434\u0430\u0440\u044f \u0432\u0438. \u041d\u044f\u043c\u0430 \u0434\u0430 \u0432\u0438 \u0440\u0430\u0437\u043e\u0447\u0430\u0440\u043e\u0432\u0430\u043c." },
                new DecisionOutcome { budgetChange = 0, moraleChange = -20, peopleChange = -1,
                    outcomeText = "\u0420\u0430\u0437\u0431\u0438\u0440\u0430\u043c. \u0411\u043b\u0430\u0433\u043e\u0434\u0430\u0440\u044f \u0437\u0430 \u0432\u0440\u0435\u043c\u0435\u0442\u043e." }));

            // 2. Kalin - Firing
            requests.Add(CreateRequest("KalinRequest", emp["Kalin"], RequestCategory.Firing,
                "\u0415\u0439, \u0448\u0435\u0444\u0435\u2026 \u0434\u0430 \u0441\u0438 \u0433\u043e\u0432\u043e\u0440\u0438\u043c \u043a\u0430\u0442\u043e \u043c\u044a\u0436\u0435, \u0430? \u0410\u0437 \u0441\u044a\u043c \u043d\u0430\u0439-\u0434\u043e\u0431\u0440\u0438\u044f\u0442 \u0434\u0435\u0432 \u0442\u0443\u043a\u0430.\n\n\u0418\u043c\u0430\u043c \u043f\u043b\u0430\u043d \u043a\u0430\u043a \u0434\u0430 \u0441\u043f\u0435\u0441\u0442\u0438\u043c \u043f\u0430\u0440\u0438. \u041c\u0430\u0445\u0430\u0442\u0435 \u0432\u0441\u0438\u0447\u043a\u0438 \u0434\u0440\u0443\u0433\u0438 \u043f\u0440\u043e\u0433\u0440\u0430\u043c\u0438\u0441\u0442\u0438.\n\n*\u0425\u043b\u044a\u0446* \u041e\u0441\u0442\u0430\u0432\u044f\u0442\u0435 \u0441\u0430\u043c\u043e \u043c\u0435\u043d, \u0438 \u0440\u0435\u0430\u043b\u043d\u043e \u0430\u0437 \u0449\u0435 \u0432\u0438 \u043e\u043f\u0440\u0430\u0432\u044f \u043f\u0440\u043e\u0435\u043a\u0442\u0430.\n\n\u0421\u0442\u0438\u0433\u0430 \u0441\u043c\u0435 \u0445\u0440\u0430\u043d\u0438\u043b\u0438 \u0447\u0435\u0442\u0438\u0440\u0438\u043c\u0430 \u0434\u0443\u0448\u0438 \u0437\u0430 \u0440\u0430\u0431\u043e\u0442\u0430, \u043a\u043e\u044f\u0442\u043e \u0435\u0434\u0438\u043d \u0447\u0438\u0442\u0430\u0432 \u043c\u043e\u0436\u0435 \u0434\u0430 \u0441\u0432\u044a\u0440\u0448\u0438.\n\n\u0410\u043a\u043e \u043d\u0435 \u0441\u0442\u0435 \u0441\u044a\u0433\u043b\u0430\u0441\u043d\u0438\u2026 \u0410\u0437 \u0441\u0438 \u0442\u0440\u044a\u0433\u0432\u0430\u043c. \u041d\u044f\u043c\u0430 \u0434\u0430 \u0441\u0435 \u0443\u043d\u0438\u0436\u0430\u0432\u0430\u043c. *\u0425\u043b\u044a\u0446*",
                new DecisionOutcome { budgetChange = 20000, moraleChange = -10, peopleChange = -4,
                    outcomeText = "\u0411\u043b\u0430\u0433\u043e\u0434\u0430\u0440\u044f \u0432\u0438. \u041d\u044f\u043c\u0430 \u0434\u0430 \u0432\u0438 \u0440\u0430\u0437\u043e\u0447\u0430\u0440\u043e\u0432\u0430\u043c." },
                new DecisionOutcome { budgetChange = 0, moraleChange = 6, peopleChange = -1,
                    outcomeText = "\u0412\u0430\u0448 \u043f\u0440\u043e\u0431\u043b\u0435\u043c." }));

            // 3. Marin - StrategicInvestment
            requests.Add(CreateRequest("MarinRequest", emp["Marin"], RequestCategory.StrategicInvestment,
                "\u0418\u0434\u0432\u0430\u043c \u0441 \u043f\u0440\u0435\u0434\u043b\u043e\u0436\u0435\u043d\u0438\u0435, \u043a\u043e\u0435\u0442\u043e \u043c\u043e\u0436\u0435 \u0434\u0430 \u043f\u0440\u043e\u043c\u0435\u043d\u0438 \u043d\u0430\u0447\u0438\u043d\u0430 \u043d\u0438 \u043d\u0430 \u0440\u0430\u0431\u043e\u0442\u0430.\n\n\u041f\u0440\u043e\u0443\u0447\u0438\u0445 \u043f\u0430\u0437\u0430\u0440\u0430 \u0438 \u0433\u043e\u0432\u043e\u0440\u0438\u0445 \u0441 \u0445\u043e\u0440\u0430. \u0410\u043a\u043e \u0438\u043d\u0432\u0435\u0441\u0442\u0438\u0440\u0430\u043c\u0435 \u0432 AI \u0438\u043d\u0441\u0442\u0440\u0443\u043c\u0435\u043d\u0442\u0438 \u0437\u0430 \u043f\u0440\u043e\u0433\u0440\u0430\u043c\u0438\u0440\u0430\u043d\u0435, \u0430\u0440\u0442 \u0438 \u0434\u0438\u0437\u0430\u0439\u043d, \u0449\u0435 \u043e\u0431\u043b\u0435\u043a\u0447\u0438\u043c \u043f\u0440\u043e\u0446\u0435\u0441\u0430 \u0438 \u0449\u0435 \u0432\u0434\u0438\u0433\u043d\u0435\u043c \u0441\u043a\u043e\u0440\u043e\u0441\u0442\u0442\u0430.\n\n\u0418\u043c\u0430 \u0440\u0438\u0441\u043a, \u0440\u0430\u0437\u0431\u0438\u0440\u0430 \u0441\u0435. \u041f\u043e \u043c\u043e\u044f \u043f\u0440\u0435\u0446\u0435\u043d\u043a\u0430 \u0438\u043c\u0430 80% \u0448\u0430\u043d\u0441 \u0434\u0430 \u0443\u0441\u043f\u0435\u0435, 20% \u0434\u0430 \u0441\u0435 \u043f\u0440\u043e\u0432\u0430\u043b\u0438.\n\n\u0422\u0440\u044f\u0431\u0432\u0430\u0442 \u043c\u0438 10 000\u20ac \u0431\u044e\u0434\u0436\u0435\u0442 \u043e\u0449\u0435 \u0441\u0435\u0433\u0430 \u0437\u0430 \u043b\u0438\u0446\u0435\u043d\u0437, \u0438\u043d\u0442\u0435\u0433\u0440\u0430\u0446\u0438\u044f \u0438 \u043e\u0431\u0443\u0447\u0435\u043d\u0438\u0435. \u041e\u0434\u043e\u0431\u0440\u044f\u0432\u0430\u0442\u0435 \u043b\u0438?",
                new DecisionOutcome { budgetChange = -10000, moraleChange = 0, peopleChange = 0,
                    outcomeText = "\u0427\u0443\u0434\u0435\u0441\u043d\u043e. \u0411\u043b\u0430\u0433\u043e\u0434\u0430\u0440\u044f \u0437\u0430 \u0434\u043e\u0432\u0435\u0440\u0438\u0435\u0442\u043e." },
                new DecisionOutcome { budgetChange = 0, moraleChange = 0, peopleChange = 0,
                    outcomeText = "\u0420\u0430\u0437\u0431\u0438\u0440\u0430\u043c. \u041d\u044f\u043c\u0430 \u0434\u0430 \u043d\u0430\u0441\u0442\u043e\u044f\u0432\u0430\u043c." }));

            // 4. Evgeniy - Hiring
            requests.Add(CreateRequest("EvgeniyRequest", emp["Evgeniy"], RequestCategory.Hiring,
                "\u0417\u0434\u0440\u0430\u0432\u0435\u0439\u0442\u0435! \u0418\u043c\u0430\u043c \u0435\u0434\u043d\u043e \u043f\u0440\u0435\u0434\u043b\u043e\u0436\u0435\u043d\u0438\u0435, \u043a\u043e\u0435\u0442\u043e \u043c\u043e\u0436\u0435 \u0434\u0430 \u043d\u0438 \u043f\u043e\u043c\u043e\u0433\u043d\u0435 \u0432 \u0435\u043a\u0438\u043f\u0430.\n\n\u0415\u0434\u0438\u043d \u043c\u043e\u0439 \u043f\u0440\u0438\u044f\u0442\u0435\u043b \u0442\u044a\u0440\u0441\u0438 \u043d\u0435\u043f\u043b\u0430\u0442\u0435\u043d \u0441\u0442\u0430\u0436 \u0438 \u0435 \u0442\u043e\u0447\u043d\u043e \u0437\u0430 \u043d\u0430\u0448\u0438\u044f \u043f\u0440\u043e\u0435\u043a\u0442.\n\n\u041d\u044f\u043c\u0430 \u0434\u0430 \u0432\u0438 \u0433\u0443\u0431\u044f \u0432\u0440\u0435\u043c\u0435\u0442\u043e \u0441\u044a\u0441 CV-\u0442\u0430 \u0438 \u0433\u043b\u0443\u043f\u043e\u0441\u0442\u0438, \u0433\u0430\u0440\u0430\u043d\u0442\u0438\u0440\u0430\u043c \u0437\u0430 \u043d\u0435\u0433\u043e.\n\n\u041c\u043e\u0436\u0435 \u043b\u0438 \u0434\u0430 \u0433\u043e \u0432\u0437\u0435\u043c\u0435\u043c \u0432 \u0435\u043a\u0438\u043f\u0430? \u0429\u0435 \u0432\u043b\u0435\u0437\u0435 \u0432\u0435\u0434\u043d\u0430\u0433\u0430 \u0432 \u0440\u0438\u0442\u044a\u043c.",
                new DecisionOutcome { budgetChange = 0, moraleChange = 2, peopleChange = 1,
                    outcomeText = "\u0421\u0443\u043f\u0435\u0440! \u041d\u044f\u043c\u0430 \u0434\u0430 \u0441\u044a\u0436\u0430\u043b\u044f\u0432\u0430\u0442\u0435." },
                new DecisionOutcome { budgetChange = 0, moraleChange = -2, peopleChange = 0,
                    outcomeText = "\u041e\u043a\u0435\u0439\u2026 \u0440\u0430\u0437\u0431\u0438\u0440\u0430\u043c." }));

            // 5. Denislava - StrategicInvestment
            requests.Add(CreateRequest("DenislavaRequest", emp["Denislava"], RequestCategory.StrategicInvestment,
                "\u0417\u0434\u0440\u0430\u0432\u0435\u0439\u0442\u0435. \u0418\u043c\u0430\u043c \u043f\u0440\u0435\u0434\u043b\u043e\u0436\u0435\u043d\u0438\u0435 \u0437\u0430 \u0441\u0442\u0430\u0431\u0438\u043b\u0438\u0437\u0438\u0440\u0430\u043d\u0435 \u043d\u0430 \u0431\u044e\u0434\u0436\u0435\u0442\u0430.\n\n\u0418\u0441\u043a\u0430\u043c \u0432\u0440\u0435\u043c\u0435\u043d\u043d\u043e \u0434\u0430 \u0441\u043f\u0440\u0435\u043c \u0431\u043e\u043d\u0443\u0441\u0438\u0442\u0435 \u0438 \u0434\u043e\u043f\u044a\u043b\u043d\u0438\u0442\u0435\u043b\u043d\u0438\u0442\u0435 \u043f\u0440\u0438\u0434\u043e\u0431\u0438\u0432\u043a\u0438 \u0437\u0430 \u0442\u043e\u0437\u0438 \u043c\u0435\u0441\u0435\u0446.\n\n\u0429\u0435 \u0441\u043f\u0435\u0441\u0442\u0438\u043c \u0434\u043e\u0441\u0442\u0430\u0442\u044a\u0447\u043d\u043e, \u0437\u0430 \u0434\u0430 \u043d\u0435 \u0440\u0435\u0436\u0435\u043c \u0437\u0430\u043f\u043b\u0430\u0442\u0438 \u0438 \u0434\u0430 \u043d\u0435 \u0437\u0430\u043a\u0440\u0438\u0432\u0430\u043c\u0435 \u043f\u043e\u0437\u0438\u0446\u0438\u0438.\n\n\u0414\u0430 \u0433\u043e \u043d\u0430\u043f\u0440\u0430\u0432\u0438\u043c \u043b\u0438, \u0438\u043b\u0438 \u043f\u043e\u0435\u043c\u0430\u043c\u0435 \u0440\u0438\u0441\u043a\u0430 \u0431\u044e\u0434\u0436\u0435\u0442\u044a\u0442 \u0434\u0430 \u043f\u0430\u0434\u043d\u0435 \u043a\u0440\u0438\u0442\u0438\u0447\u043d\u043e?",
                new DecisionOutcome { budgetChange = 12000, moraleChange = -6, peopleChange = 0,
                    outcomeText = "\u0414\u043e\u0431\u0440\u0435. \u0429\u0435 \u0433\u043e \u043e\u0444\u043e\u0440\u043c\u044f \u043e\u0444\u0438\u0446\u0438\u0430\u043b\u043d\u043e \u0438 \u0449\u0435 \u0433\u043e \u043a\u043e\u043c\u0443\u043d\u0438\u043a\u0438\u0440\u0430\u043c \u0432\u043d\u0438\u043c\u0430\u0442\u0435\u043b\u043d\u043e." },
                new DecisionOutcome { budgetChange = -10000, moraleChange = 4, peopleChange = -1,
                    outcomeText = "\u0420\u0430\u0437\u0431\u0438\u0440\u0430\u043c. \u0422\u043e\u0433\u0430\u0432\u0430 \u0449\u0435 \u0442\u0440\u044f\u0431\u0432\u0430 \u0434\u0430 \u0441\u043c\u0435 \u043c\u043d\u043e\u0433\u043e \u0432\u043d\u0438\u043c\u0430\u0442\u0435\u043b\u043d\u0438 \u0441 \u043e\u0441\u0442\u0430\u043d\u0430\u043b\u0438\u0442\u0435 \u0440\u0435\u0448\u0435\u043d\u0438\u044f \u0434\u043d\u0435\u0441." }));

            // 6. Trifon - StrategicInvestment
            requests.Add(CreateRequest("TrifonRequest", emp["Trifon"], RequestCategory.StrategicInvestment,
                "\u0417\u0434\u0440\u0430\u0432\u0435\u0439\u0442\u0435. \u041d\u044f\u043c\u0430 \u0434\u0430 \u0433\u043e \u0443\u0432\u044a\u0440\u0442\u0430\u043c.\n\n\u0410\u0440\u0442 \u0435\u043a\u0438\u043f\u044a\u0442 \u0435 \u0437\u0430\u0442\u0440\u0443\u043f\u0430\u043d\u2026 \u0412\u0441\u0438\u0447\u043a\u043e \u0435 '\u0441\u043f\u0435\u0448\u043d\u043e'.\n\n\u0410\u043a\u043e \u043f\u0440\u043e\u0434\u044a\u043b\u0436\u0438\u043c \u0442\u0430\u043a\u0430, \u0438\u043b\u0438 \u0449\u0435 \u043f\u0430\u0434\u043d\u0435 \u043a\u0430\u0447\u0435\u0441\u0442\u0432\u043e\u0442\u043e, \u0438\u043b\u0438 \u0445\u043e\u0440\u0430\u0442\u0430 \u0449\u0435 \u043f\u0440\u0435\u0433\u043e\u0440\u044f\u0442.\n\n\u0418\u0441\u043a\u0430\u043c \u0434\u0430 \u043e\u0440\u0435\u0436\u0435\u043c \u0447\u0430\u0441\u0442 \u043e\u0442 \u0437\u0430\u0434\u0430\u0447\u0438\u0442\u0435 \u0438 \u0434\u0430 \u0437\u0430\u043a\u043b\u044e\u0447\u0438\u043c \u0430\u0440\u0442 \u0441\u0442\u0438\u043b\u0430 \u0437\u0430 \u043e\u0441\u0442\u0430\u0442\u044a\u043a\u0430 \u043e\u0442 \u043f\u0440\u043e\u0435\u043a\u0442\u0430. \u041e\u0434\u043e\u0431\u0440\u044f\u0432\u0430\u0442\u0435 \u043b\u0438?",
                new DecisionOutcome { budgetChange = -5000, moraleChange = 4, peopleChange = 0,
                    outcomeText = "\u0411\u043b\u0430\u0433\u043e\u0434\u0430\u0440\u044f. \u0422\u043e\u0432\u0430 \u0449\u0435 \u043d\u0438 \u0441\u043f\u0430\u0441\u0438. \u041f\u043e-\u0434\u043e\u0431\u0440\u0435 \u043f\u043e-\u043c\u0430\u043b\u043a\u043e, \u043d\u043e \u043a\u0430\u043a\u0442\u043e \u0442\u0440\u044f\u0431\u0432\u0430." },
                new DecisionOutcome { budgetChange = 0, moraleChange = -4, peopleChange = -2,
                    outcomeText = "\u042f\u0441\u043d\u043e\u2026 \u0442\u043e\u0433\u0430\u0432\u0430 \u0449\u0435 '\u0431\u0443\u0442\u0430\u043c\u0435'. \u0421\u0430\u043c\u043e \u043d\u0435 \u043c\u0435 \u043f\u0438\u0442\u0430\u0439\u0442\u0435 \u043f\u043e\u0441\u043b\u0435 \u0437\u0430\u0449\u043e \u043d\u0435 \u0438\u0437\u0433\u043b\u0435\u0436\u0434\u0430 \u043a\u0430\u0442\u043e \u0432 \u0442\u0440\u0435\u0439\u043b\u044a\u0440\u0430." }));

            // 7. MarinRequestYes — only appears if MarinRequest was approved
            var marinRequest = requests[2]; // MarinRequest is index 2
            requests.Add(CreateRequest("MarinRequestYes", emp["Marin"], RequestCategory.StrategicInvestment,
                "\u0412\u044a\u0440\u043d\u0430\u0445 \u0441\u0435 \u0441 \u043d\u043e\u0432\u0438\u043d\u0430. \u0418\u043d\u0432\u0435\u0441\u0442\u0438\u0442\u043e\u0440\u0438\u0442\u0435 \u0438\u0437\u043b\u044f\u0437\u043e\u0445\u0430 \u0441 \u043a\u043e\u043d\u043a\u0440\u0435\u0442\u0435\u043d \u043f\u043b\u0430\u043d \u0438 \u043f\u043e\u0435\u0445\u0430 \u0447\u0430\u0441\u0442 \u043e\u0442 \u0432\u043d\u0435\u0434\u0440\u044f\u0432\u0430\u043d\u0435\u0442\u043e.\n\n\u0421 \u0434\u0440\u0443\u0433\u0438 \u0434\u0443\u043c\u0438 \u0440\u0430\u0431\u043e\u0442\u0438. \u0423\u0441\u043f\u044f\u0445 \u0434\u0430 \u0443\u0431\u0435\u0434\u044f \u0438\u043d\u0432\u0435\u0441\u0442\u0438\u0442\u043e\u0440\u0438\u0442\u0435, \u0438 \u043d\u0438 \u0432\u0440\u044a\u0447\u0438\u0445\u0430 20.000\u20ac!\n\n\u0429\u0435 \u043d\u0430\u0437\u043d\u0430\u0447\u0438\u043c \u043e\u0449\u0435 \u0445\u043e\u0440\u0430 \u0437\u0430 \u0434\u0430 \u0434\u0432\u0438\u0436\u0430\u0442 \u043d\u043e\u0432\u0438\u044f AI \u043e\u0442\u0434\u0435\u043b. \u041e\u0434\u043e\u0431\u0440\u044f\u0432\u0430\u0442\u0435 \u043b\u0438?",
                new DecisionOutcome { budgetChange = 20000, moraleChange = 0, peopleChange = 2,
                    outcomeText = "\u041e\u0442\u043b\u0438\u0447\u043d\u043e! \u041f\u043e\u0435\u043c\u0430\u043c\u0435 \u043d\u0430\u043f\u0440\u0435\u0434." },
                new DecisionOutcome { budgetChange = 10000, moraleChange = 0, peopleChange = 0,
                    outcomeText = "\u0420\u0430\u0437\u0431\u0438\u0440\u0430\u043c. \u0429\u0435 \u043f\u0440\u0435\u0434\u0430\u043c \u043d\u0430 \u0438\u043d\u0432\u0435\u0441\u0442\u0438\u0442\u043e\u0440\u0438\u0442\u0435." },
                requiresApproved: new[] { marinRequest }
            ));

            // 8. MarinRequestNo — only appears if MarinRequest was denied
            requests.Add(CreateRequest("MarinRequestNo", emp["Marin"], RequestCategory.StrategicInvestment,
                "\u0422\u0440\u044f\u0431\u0432\u0430\u0448\u0435 \u0434\u0430 \u0438\u043d\u0432\u0435\u0441\u0442\u0438\u0440\u0430\u043c\u0435. \u041f\u0440\u043e\u0437\u043e\u0440\u0435\u0446\u044a\u0442 \u0441\u0435 \u0437\u0430\u0442\u0432\u043e\u0440\u0438.\n\n\u0427\u0430\u0441\u0442 \u043e\u0442 \u0445\u043e\u0440\u0430\u0442\u0430 \u0433\u043e \u043f\u0440\u0438\u0435\u0445\u0430 \u043a\u0430\u0442\u043e \u0437\u043d\u0430\u043a, \u0447\u0435 \u043f\u0440\u043e\u0446\u0435\u0441\u044a\u0442 \u043d\u0438\u043a\u043e\u0433\u0430 \u043d\u044f\u043c\u0430 \u0434\u0430 \u0441\u0442\u0430\u043d\u0435 \u043f\u043e-\u043b\u0435\u043a. \u0412 \u043c\u043e\u043c\u0435\u043d\u0442\u0430 \u043c\u043e\u0440\u0430\u043b\u044a\u0442 \u043f\u0430\u0434\u0430.",
                new DecisionOutcome { budgetChange = -5000, moraleChange = -5, peopleChange = 0,
                    outcomeText = "\u0421\u044a\u0436\u0430\u043b\u044f\u0432\u0430\u043c\u0435. \u0421\u043b\u0435\u0434\u0432\u0430\u0449\u0438\u044f \u043f\u044a\u0442 \u0449\u0435 \u0431\u044a\u0434\u0435\u043c \u043f\u043e-\u0432\u043d\u0438\u043c\u0430\u0442\u0435\u043b\u043d\u0438." },
                new DecisionOutcome { budgetChange = -5000, moraleChange = -5, peopleChange = 0,
                    outcomeText = "\u042f\u0441\u043d\u043e. \u0429\u0435 \u0433\u043e \u0432\u0437\u0435\u043c\u0430 \u043f\u0440\u0435\u0434\u0432\u0438\u0434." },
                requiresDenied: new[] { marinRequest }
            ));

            // 9. Debora - Hiring
            requests.Add(CreateRequest("DeboraRequest", emp["Debora"], RequestCategory.Hiring,
                "\u0417\u0434\u0440\u0430\u0432\u0435\u0439\u0442\u0435. \u0418\u043c\u0430\u043c \u043a\u043e\u043d\u043a\u0440\u0435\u0442\u043d\u0430 \u043d\u0443\u0436\u0434\u0430.\n\nQA \u0435\u043a\u0438\u043f\u044a\u0442 \u043d\u0435 \u0441\u043c\u043e\u0433\u0432\u0430\u2026 \u0411\u0440\u043e\u044f\u0442 \u0442\u0435\u0441\u0442\u043e\u0432\u0435 \u0440\u0430\u0441\u0442\u0435, \u0430 \u0445\u043e\u0440\u0430\u0442\u0430 \u0441\u0430 \u0441\u044a\u0449\u0438\u0442\u0435.\n\n\u0422\u0440\u044f\u0431\u0432\u0430\u0442 \u043c\u0438 \u043e\u0449\u0435 3-\u043c\u0430 \u0434\u0443\u0448\u0438 \u0432 QA, \u0438\u043d\u0430\u0447\u0435 \u0449\u0435 \u0438\u0437\u043f\u0443\u0441\u043d\u0435\u043c \u043a\u0440\u0438\u0442\u0438\u0447\u043d\u0438 \u0431\u044a\u0433\u043e\u0432\u0435 \u0432 \u0440\u0435\u043b\u0438\u0439\u0437.\n\n\u041c\u043e\u0433\u0430 \u0434\u0430 \u0433\u0438 \u043e\u0431\u0443\u0447\u0430 \u0431\u044a\u0440\u0437\u043e. \u041e\u0434\u043e\u0431\u0440\u044f\u0432\u0430\u0442\u0435 \u043b\u0438 \u0434\u0430 \u0433\u0438 \u043d\u0430\u0435\u043c\u0435\u043c?",
                new DecisionOutcome { budgetChange = -6000, moraleChange = 2, peopleChange = 5,
                    outcomeText = "\u0411\u043b\u0430\u0433\u043e\u0434\u0430\u0440\u044f. \u0422\u043e\u0432\u0430 \u0440\u0435\u0430\u043b\u043d\u043e \u0449\u0435 \u043d\u0438 \u0441\u043f\u0430\u0441\u0438 \u0440\u0435\u043b\u0438\u0439\u0437\u0430." },
                new DecisionOutcome { budgetChange = 0, moraleChange = -3, peopleChange = 0,
                    outcomeText = "\u0420\u0430\u0437\u0431\u0438\u0440\u0430\u043c. \u0429\u0435 \u043d\u0430\u043f\u0440\u0430\u0432\u0438\u043c \u043a\u0430\u043a\u0432\u043e\u0442\u043e \u043c\u043e\u0436\u0435\u043c \u0441 \u043d\u0430\u043b\u0438\u0447\u043d\u0438\u0442\u0435 \u0445\u043e\u0440\u0430." }));

            return requests;
        }
    }
}
