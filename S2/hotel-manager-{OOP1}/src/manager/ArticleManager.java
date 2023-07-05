package manager;

import database.DataBase;
import entity.Article;
import entity.price.Price;
import entity.price.PriceSet;
import entity.user.User;
import main.Settings;
import manager.generic.NumericManager;
import type.DateRange;
import util.InternalUtil;

import java.io.IOException;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.Comparator;
import java.util.HashSet;
import java.util.LinkedHashMap;

public class ArticleManager extends NumericManager {

    public ArticleManager() {
        super();
    }

    public ArticleManager(String articlesPath) throws IOException {
        super(articlesPath);
    }

    @Override
    protected void updateIdLength(Integer nextId) {
        // ne bi trebalo da se desi u realnim uslovima
        Settings.articleIdLength = Math.max(String.valueOf(nextId).length(), Settings.articleIdLength);
    }


    public Article getArticle(int id) {
        return (Article) get(id);
    }

    public Integer addArticle(Article article) {
        Integer newId = article.getId();
        if (newId == 0) { // ceka se da sistem dodeli Id
            newId = getNextId();
            article.setId(newId);
        }
        if (uniqueTitle(article) &&
                add(newId, article)) {
            return newId;
        }
        return 0;
    }

    public boolean updateArticle(Integer id, Article article) {
        remove(id);
        return add(article.getId(), article);
    }

    public boolean removeArticle(int id) {
        return remove(id);
    }

    public void clear() {
        reset();
    }

    public boolean uniqueTitle(Article article) {
        String title = article.getTitle();
        for (Object value : values()) {
            Article a = (Article) value;
            if (a.getTitle().equals(title)) {
                return false;
            }
        }
        return true;
    }

    public LinkedHashMap<Integer, String> getTitles() {
        sort((o1, o2) -> {
            Article a1 = (Article) o1;
            Article a2 = (Article) o2;
            return a1.getTitle().compareTo(a2.getTitle());
        });

        LinkedHashMap<Integer, String> titles = new LinkedHashMap<>();
        for (Object value : values()) {
            Article a = (Article) value;
            titles.put(a.getId(), a.getTitle());
        }
        return titles;
    }

    @Override
    public void fetchData(String filepath) throws IOException {
        HashSet<Integer> validTokenLength = new HashSet<>();
        validTokenLength.add(3);
        ArrayList<String[]> allTokens = DataBase.readTokens(filepath, validTokenLength);

        Article newArticle;
        for (String[] tokens : allTokens) {
            int id = InternalUtil.parseInt(tokens[0]);
            String title = tokens[1];

            String[] priceSetTokens = tokens[2].split(Settings.delimiterB, -1);
            PriceSet priceSet = new PriceSet();

            for (String set : priceSetTokens) {
                if (set.isEmpty())
                    continue;
                String[] setTokens = set.split(Settings.delimiterC, -1);
                double amount = InternalUtil.parseDouble(setTokens[0].trim());
                LocalDate start = InternalUtil.parseDate(setTokens[1].trim());
                LocalDate end = InternalUtil.parseDate(setTokens[2].trim());
                priceSet.add(new Price(amount, new DateRange(start, end)));
            }

            newArticle = new Article(id, title, priceSet);
            add(id, newArticle);
        }
    }

    @Override
    public void saveData(String filepath) throws IOException {
        ArrayList<String[]> allTokens = new ArrayList<>();

        sort(Comparator.comparing(o -> ((Article) o).getId()));
        for (Object value : values()) {
            Article article = (Article) value;

            ArrayList<String> priceSetTokens = new ArrayList<>();
            for (Price price : article.getPriceSet().getPrices()) {
                String amount = Double.toString(price.getAmount());
                String start = InternalUtil.toString(price.getDateRange().getStart());
                String end = InternalUtil.toString(price.getDateRange().getEnd());

                priceSetTokens.add(String.join(Settings.delimiterC, amount, start, end));
            }

            String[] tokens = new String[]{
                    InternalUtil.toString(article.getId()),
                    article.getTitle(),
                    String.join(Settings.delimiterB, priceSetTokens)
            };
            allTokens.add(tokens);
        }
        DataBase.writeTokens(filepath, allTokens);
    }
}
