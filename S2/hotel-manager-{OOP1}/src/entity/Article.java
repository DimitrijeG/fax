package entity;

import entity.price.Price;
import entity.price.PriceSet;
import type.DateRange;

import java.time.LocalDate;

public class Article {
    int id;
    String title;
    PriceSet priceSet;

    public Article() {
        id = 0;
        title = "";
        priceSet = new PriceSet();
    }

    public Article(String title, PriceSet priceSet) {
        this();
        this.title = title;
        this.priceSet = priceSet;
    }

    public Article(int id, String title, PriceSet priceSet) {
        this(title, priceSet);
        this.id = id;
    }

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public PriceSet getPriceSet() {
        return priceSet;
    }

    public void setPriceSet(PriceSet priceSet) {
        this.priceSet = priceSet;
    }

    public double calculatePrice(LocalDate start) {
        for (Price p : priceSet.getPrices()) {
            DateRange r = p.getDateRange();
            if (r.contains(start))
                return p.getAmount();
        }
        return 0;
    }

    public double calculatePrice(DateRange range) {
        double price = 0;
        for (LocalDate d : range.getDates()) {
            for (Price p : priceSet.getPrices())
                if (p.getDateRange().contains(d)) {
                    price += p.getAmount();
                    break;
                }
        }
        return price;
    }

    public Article copy() {
        return new Article(id, title, priceSet);
    }
}
