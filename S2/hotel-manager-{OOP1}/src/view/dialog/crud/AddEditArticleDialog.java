package view.dialog.crud;

import entity.Article;
import entity.price.Price;
import entity.price.PriceSet;
import main.Settings;
import manager.ArticleManager;
import net.miginfocom.swing.MigLayout;
import type.DateException;
import type.DateRange;
import type.ExistingIdException;
import type.ValidatorException;
import util.Validate;
import util.ViewUtil;
import view.dialog.MessageDialog;
import view.generic.InputForm;

import javax.swing.*;
import java.awt.*;
import java.time.LocalDate;
import java.util.ArrayList;

import static util.ViewUtil.*;

public class AddEditArticleDialog extends JDialog implements InputForm {
    private final ArticleManager articleManager;
    private final Article initData;
    private Integer nextId = 0;
    private final JTextField idField = new JTextField(), titleField = new JTextField();
    private final JTextArea priceSetArea = new JTextArea();
    private final String delimiter = ";";
    private final String title;

    public AddEditArticleDialog(JFrame parent, ArticleManager articleManager, Article article, String title) {
        super(parent, true);
        this.articleManager = articleManager;
        this.initData = article;
        this.title = title;

        setDefaultCloseOperation(JFrame.DISPOSE_ON_CLOSE);
        setLocationRelativeTo(null);
        initialize();
    }

    @Override
    public void initialize() {
        LayoutManager dialogLayout = new MigLayout(
                "wrap 2,insets 15 30 15 30",
                "[::,right]10[:220:,left]",
                "[]8[]8[:180:,top]12[]");
        setLayout(dialogLayout);

//        ------------------------------------------------------------

        add(new Label("Id:"));
        add(idField, "growx");
        add(new Label("Naziv:"));
        add(titleField, "growx");
        add(new Label("Cene:"), "gaptop 25");
        add(priceSetArea, "growx,growy");

        add(failStatus, "growx,span 2,align center");

        if (initData == null) {
            nextId = articleManager.getNextId();
            idField.setText(nextId.toString());
            setTitle("Dodavanje " + title);
            add(initAdd("Dodaj"));
        } else {
            setTitle("Izmena " + title);
            add(initEdit("Izmeni"));
        }
        add(cancelButton);

        changeFont(this, Settings.font);
        pack();
        setVisible(true);
    }

    @Override
    public void validateFields() throws ValidatorException {
        Integer id = parseInteger(getText(idField));
        Integer suggestedId = nextId;
        if (initData == null || !id.equals(initData.getId())) {
            try {
                Validate.newId(getText(idField), articleManager.keys());
            } catch (ExistingIdException e) {
                if (initData != null)
                    suggestedId = initData.getId();
                if (MessageDialog.suggestId(this, suggestedId, e.getMessage()))
                    idField.setText(suggestedId.toString());
            }
        }
        Validate.notEmpty(getText(titleField));
        String title = getText(titleField);
        if (initData == null || !title.equals(initData.getTitle()))
            Validate.newUsername(title, articleManager.getTitles().values(), "Naziv mora da bude unikatan.");

        PriceSet priceSet = new PriceSet();
        String[] lines = getText(priceSetArea).split("\n");
        String[] tokens;
        for (String line : lines) {
            if (line.isEmpty())
                continue;
            tokens = line.split(delimiter);
            if (tokens.length != 3)
                throw new ValidatorException("Neispravne cene.");
            Validate.price(tokens[0]);
            Validate.localDate(tokens[1]);
            Validate.localDate(tokens[2]);
            LocalDate d1 = ViewUtil.parseDate(tokens[1]);
            LocalDate d2 = ViewUtil.parseDate(tokens[2]);
            if (!d1.isBefore(d2)) throw new DateException("Početni datum cene mora biti pre krajnjeg");
            if (!priceSet.add(new Price(
                    ViewUtil.parseDouble(tokens[0]), new DateRange(
                    d1,
                    d2))))
                throw new ValidatorException();
        }
    }

    @Override
    public void addObject() {
        Article a = (Article) collectData();
        articleManager.addArticle(a);
    }

    @Override
    public void updateObject() {
        Article a = (Article) collectData();
        articleManager.updateArticle(initData.getId(), a);
    }

    @Override
    public Object collectData() {
        Integer id = parseInteger(getText(idField));
        String title = getText(titleField);
        PriceSet priceSet = new PriceSet();
        String[] lines = getText(priceSetArea).split("\n");
        String[] tokens;
        for (String line : lines) {
            if (line.isEmpty())
                continue;
            tokens = line.split(delimiter);
            double amount = ViewUtil.parseDouble(tokens[0].trim());
            LocalDate start = ViewUtil.parseDate(tokens[1].trim());
            LocalDate end = ViewUtil.parseDate(tokens[2].trim());
            priceSet.add(new Price(amount, new DateRange(start, end)));
        }

        return new Article(id, title, priceSet);
    }

    @Override
    public void populate() {
        idField.setText(initData.getId().toString());
        titleField.setText(initData.getTitle());
        ArrayList<String> lines = new ArrayList<>();
        String[] tokens;
        for (Price price : initData.getPriceSet().getPrices()) {
            tokens = new String[]{
                    ViewUtil.toString(price.getAmount()),
                    ViewUtil.toString(price.getDateRange().getStart()),
                    ViewUtil.toString(price.getDateRange().getEnd())
            };
            lines.add(String.join(delimiter, tokens));
        }
        priceSetArea.setText(String.join("\n", lines));
    }

    @Override
    public boolean confirmEdit() {
        return MessageDialog.confirm(this, "Potvrđujete unesene izmene?");
    }

    @Override
    public void close() {
        dispose();
    }
}
