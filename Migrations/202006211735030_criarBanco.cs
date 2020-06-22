namespace MePague.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class criarBanco : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.amizades",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        status = c.String(),
                        solicitado_id = c.Int(),
                        solicitante_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.clientes", t => t.solicitado_id)
                .ForeignKey("dbo.clientes", t => t.solicitante_id)
                .Index(t => t.solicitado_id)
                .Index(t => t.solicitante_id);
            
            CreateTable(
                "dbo.clientes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nome = c.String(nullable: false),
                        email = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.dependentes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        idDependente = c.Int(nullable: false),
                        valor = c.Double(nullable: false),
                        status = c.String(),
                        dependente_id = c.Int(),
                        Despesa_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.clientes", t => t.dependente_id)
                .ForeignKey("dbo.despesas", t => t.Despesa_id)
                .Index(t => t.dependente_id)
                .Index(t => t.Despesa_id);
            
            CreateTable(
                "dbo.despesas",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nome = c.String(nullable: false),
                        valor = c.Double(nullable: false),
                        status = c.String(),
                        tipo = c.String(),
                        dono_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.clientes", t => t.dono_id, cascadeDelete: true)
                .Index(t => t.dono_id);
            
            CreateTable(
                "dbo.sessoes",
                c => new
                    {
                        sessao = c.Int(nullable: false, identity: true),
                        usuario_id = c.Int(),
                    })
                .PrimaryKey(t => t.sessao)
                .ForeignKey("dbo.usuarios", t => t.usuario_id)
                .Index(t => t.usuario_id);
            
            CreateTable(
                "dbo.usuarios",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nome = c.String(nullable: false),
                        senha = c.String(nullable: false),
                        tipo = c.String(maxLength: 1),
                        cliente_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.clientes", t => t.cliente_id)
                .Index(t => t.cliente_id);
            
            CreateTable(
                "dbo.status",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        sigla = c.String(),
                        nome = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.sessoes", "usuario_id", "dbo.usuarios");
            DropForeignKey("dbo.usuarios", "cliente_id", "dbo.clientes");
            DropForeignKey("dbo.despesas", "dono_id", "dbo.clientes");
            DropForeignKey("dbo.dependentes", "Despesa_id", "dbo.despesas");
            DropForeignKey("dbo.dependentes", "dependente_id", "dbo.clientes");
            DropForeignKey("dbo.amizades", "solicitante_id", "dbo.clientes");
            DropForeignKey("dbo.amizades", "solicitado_id", "dbo.clientes");
            DropIndex("dbo.usuarios", new[] { "cliente_id" });
            DropIndex("dbo.sessoes", new[] { "usuario_id" });
            DropIndex("dbo.despesas", new[] { "dono_id" });
            DropIndex("dbo.dependentes", new[] { "Despesa_id" });
            DropIndex("dbo.dependentes", new[] { "dependente_id" });
            DropIndex("dbo.amizades", new[] { "solicitante_id" });
            DropIndex("dbo.amizades", new[] { "solicitado_id" });
            DropTable("dbo.status");
            DropTable("dbo.usuarios");
            DropTable("dbo.sessoes");
            DropTable("dbo.despesas");
            DropTable("dbo.dependentes");
            DropTable("dbo.clientes");
            DropTable("dbo.amizades");
        }
    }
}
