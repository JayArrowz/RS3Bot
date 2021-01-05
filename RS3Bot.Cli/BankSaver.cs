using RS3Bot.Abstractions.Model;
using RS3Bot.DAL;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RS3Bot.Cli
{
    public class BankSaver
    {
        public static async Task SaveBank(ApplicationDbContext context, ApplicationUser user, bool saveChanges = true)
        {
            try
            {
                if (saveChanges)
                {
                    context.RemoveRange(user.Items);
                    await context.SaveChangesAsync();
                }
                var items = user.Bank.GetItems()
                    .Where(t => t != null)
                    .Select(t => new UserItem
                    {
                        UserId = user.Id,
                        Item = t
                    }).ToList();
                user.Bank.UpdateRequired = false;

                context.AttachRange(items);
                context.AddRange(items);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
