# DemoAppServiceScaling

この Web アプリケーションは Azure App Service の自動スケールをデモするためのものです。

# 機能

- Request を受け付け、CPU負荷をかけるページ

```
/applyload
```

Request が着信した マシン名 （インスタンス名）と着信日時を CosmosDb に保存します。次の実装で少しCPUに負荷がかかるようになっています。

```csharp
var bytes = new byte[1024 * 1024];
for (var i = 0; i < 1024 * 1024; i++)
{
    bytes[i] = (byte)i;
}
```



- マシン（インスタンス）毎の Request 数一覧

```
/
```

マシン名（インスタンス名）毎に Request 数を集計して一覧表示します。

- リセット

```
/deleteallresults
```

CosmosDb に保存したこれまでの着信履歴をすべて削除します。

## セットアップ

Azure App Servcie と、 Azure Cosmos Db が必要です。 Azure Cosmos Db への接続は App Service のアプリケーション設定で次のように値をセットします。

| Key              | Value                                                   |
| ---------------- | ------------------------------------------------------- |
| CosmosDb:Account | https://\<your-cosmosdb-name\>.documents.azure.com:443/ |
| CosmosDb:Key     | \<your-cosmosdb-key\>                                   |

Local環境で稼働させる場合は、appSettings.Development.json ファイルをプロジェクトルートに作成し、次の例のように設定します。

```json:appSeeings.Development.json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "CosmosDb": {
    "Account": "https://<your-cosmosdb-name.documents.azure.com:443/",
    "Key": "<your-cosmosdb-key",
    "DatabaseName": "AccessHistory",
    "ContainerName": "RequestData"
  }
}
```