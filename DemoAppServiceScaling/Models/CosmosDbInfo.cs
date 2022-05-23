namespace DemoAppServiceScaling.Models
{
    public class CosmosDbInfo
    {
#pragma warning disable CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。
        public string Account { get; set; }

        public string Key { get; set; }

        public string DatabaseName { get; set; }

        public string ContainerName { get; set; }
#pragma warning restore CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。
    }
}
