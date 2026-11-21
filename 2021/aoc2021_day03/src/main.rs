fn main() {
    //let input = include_str!("../input_sample.txt");
    let input = include_str!("../input.txt");

    let mut total_rows = 0;
    let dimension_count = input.lines().next().unwrap().trim().chars().count();
    let mut one_counts = vec![0i64; dimension_count];

    for x in input.lines().map(parse_line) {
        total_rows = total_rows + 1;
        for i in 0..dimension_count {
            if x[i] { one_counts[i] = one_counts[i] + 1 }
        }
    }

    let mut gamma_rate = 0i64;
    let mut epsilon_rate = 0i64;

    for i in one_counts {
        gamma_rate = gamma_rate << 1;
        epsilon_rate = epsilon_rate << 1;

        if i * 2 > total_rows {
            gamma_rate = gamma_rate + 1;
        } else {
            epsilon_rate = epsilon_rate + 1;
        }
    }

    println!("Part 1: {gamma_rate} {epsilon_rate} => {}", gamma_rate * epsilon_rate);

    let inputs : Vec<i64> = input.lines()
        .map(parse_line)
        .map(|bools| bools.iter().fold(0, |acc, is_one| (acc << 1) + (if *is_one { 1 } else { 0 })))
        .collect();

    let oxygen_generator_rating = filter_until_single(dimension_count, &inputs, true);
    let co2_scrubber_rating = filter_until_single(dimension_count, &inputs, false);

    println!("Part 2: O2: {oxygen_generator_rating} CO2: {co2_scrubber_rating}\tResult: {}", oxygen_generator_rating * co2_scrubber_rating);
}

fn parse_line(line: &str) -> Vec<bool> {
    line.chars().map(|ch| ch == '1').collect()
}

fn filter_until_single(dimension_count: usize, inputs: &[i64], most_common: bool) -> i64 {
    let mut inputs = inputs.to_vec();
    let mut position = dimension_count;
    let mut count = inputs.iter().count();
    while count > 1 {
        position = position - 1;

        let one_count = inputs.iter().filter(|i| (**i >> position) & 1 == 1).count();
        let most_common_bit = if one_count * 2 >= count { 1 } else { 0 };
        let least_common_bit = if one_count * 2 < count { 1 } else { 0 };
        let defined_bit = if most_common { most_common_bit } else { least_common_bit };

        inputs = inputs.iter().filter(|i| (**i >> position & 1) == defined_bit).map(|f| *f).collect();
        count = inputs.iter().count();
    }

    *inputs.iter().next().unwrap()
}