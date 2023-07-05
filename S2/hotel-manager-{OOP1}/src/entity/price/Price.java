package entity.price;

import type.DateRange;

import java.time.LocalDate;

public class Price implements Comparable<Price> {
    private Double amount;
    private DateRange dateRange;

    public Price() {
        amount = 0.0;
        dateRange = new DateRange();
    }

    public Price(double amount, DateRange dateRange) {
        this.amount = amount;
        this.dateRange = dateRange;
    }

    public boolean equals(Price price) {
        return this.amount == price.amount && this.dateRange.equals(price.dateRange);
    }

    public Double getAmount() {
        return amount;
    }

    public void setAmount(Double amount) {
        this.amount = amount;
    }

    public DateRange getDateRange() {
        return dateRange;
    }

    public void setDateRange(DateRange dateRange) {
        this.dateRange = dateRange;
    }

    @Override
    public int compareTo(Price price) {
        LocalDate start1 = dateRange.getStart();
        LocalDate start2 = price.getDateRange().getStart();
        if (start1.isBefore(start2)) {
            return -1; // this je manji od onog sa kojim se poredi
        } else if (start1.isAfter(start2)) {
            return 1;
        } else {
            return amount.compareTo(price.getAmount());
        }
    }
}
