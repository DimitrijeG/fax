package view.frame.table.crud;

import entity.Article;
import manager.ArticleManager;
import table.model.ArticleModel;
import util.ViewUtil;
import view.dialog.MessageDialog;
import view.dialog.crud.AddEditArticleDialog;

import javax.swing.*;

public class ArticleTableFrame extends CRUDTableFrame {
    private final ArticleManager articleManager;

    public ArticleTableFrame(ArticleManager articleManager) {
        super(new ArticleModel(articleManager));
        this.articleManager = articleManager;
        setTitle("Artikli");
        setSize(560, 280);
    }

    @Override
    protected void initActions() {
        buttonAdd.addActionListener(e -> {
            new AddEditArticleDialog(null, articleManager, null, "artikla");
        });
        buttonEdit.addActionListener(e -> {
            int row = table.getSelectedRow();
            if (row == -1)
                MessageDialog.tableRowNotSelected();
            else {
                Integer id = ViewUtil.parseInteger(table.getValueAt(row, 0).toString());
                Article article = articleManager.getArticle(id);

                if (article != null) {
                    Article editedArticle = article.copy();
                    new AddEditArticleDialog(null, articleManager, editedArticle, "artikla");
                    updateTable();
                } else
                    MessageDialog.ok(null, "Nije moguce pronaći određeni artikal!", "Greška", JOptionPane.ERROR_MESSAGE);
            }
        });
        buttonDelete.addActionListener(e -> {
            int row = table.getSelectedRow();
            if (row == -1)
                MessageDialog.tableRowNotSelected();
            else {
                Integer id = ViewUtil.parseInteger(table.getValueAt(row, 0).toString());
                Article article = articleManager.getArticle(id);

                if (article != null) {
                    int choice = JOptionPane.showConfirmDialog(null, "Da li ste sigurni da zelite da obrisete artikal?",
                            "Potvrda brisanja", JOptionPane.YES_NO_OPTION);
                    if (choice == JOptionPane.YES_OPTION) {
                        articleManager.removeArticle(article.getId());
                        updateTable();
                    }
                } else {
                    JOptionPane.showMessageDialog(null, "Nije moguce pronaci odabrani artikal!", "Greska", JOptionPane.ERROR_MESSAGE);
                }
            }
        });
    }

    @Override
    protected void sortData(int index) {
        articleManager.sort((o1, o2) -> {
            Article a1 = (Article) o1;
            Article a2 = (Article) o2;
            int retVal = 0;
            switch (index) {
                case 0:
                    retVal = a1.getId().compareTo(a2.getId());
                    break;
                case 1:
                    retVal = a1.getTitle().compareTo(a2.getTitle());
                    break;
                case 2:
                    retVal = a1.getPriceSet().compareTo(a2.getPriceSet());
                    break;
                default:
                    System.out.println("Too many columns to sort.");
                    System.exit(1);
                    break;
            }
            return retVal * sortOrder.get(index);
        });
    }
}
