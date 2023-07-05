package table.model;

import entity.Article;
import main.Settings;
import manager.ArticleManager;
import util.ViewUtil;

import java.util.ArrayList;

public class ArticleModel extends TableModel {

    ArticleManager articleManager;

    public ArticleModel(ArticleManager articleManager) {
        super(new String[]{
                "Id", "Naziv", "Cene"
        });
        this.articleManager = articleManager;
    }

    @Override
    protected ArrayList<Object> getData() {
        return articleManager.values();
    }

    @Override
    public Object getValueAt(int rowIndex, int columnIndex) {
        Article article = (Article) articleManager.values().get(rowIndex);
        switch (columnIndex) {
            case 0:
                return ViewUtil.formatId(article.getId(), Settings.articleIdLength);
            case 1:
                return article.getTitle();
            case 2:
                return article.getPriceSet();
            default:
                return null;
        }
    }
}
