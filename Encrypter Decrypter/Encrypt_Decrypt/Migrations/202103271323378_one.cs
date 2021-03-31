namespace Encrypt_Decrypt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class one : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TextDetails",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        plaintext = c.String(),
                        encryptedtext = c.String(),
                        decryptedtext = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TextDetails");
        }
    }
}
