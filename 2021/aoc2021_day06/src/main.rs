fn main() {
    //let input = include_str!("../input_sample.txt");
    let input = include_str!("../input.txt");

    let input: Vec<usize> = input.split(',').map(|x| x.parse().unwrap()).collect();

    let mut counts_by_timer = [0i64; 9];
    for timer in input {
        counts_by_timer[timer] = counts_by_timer[timer] + 1;
    }

    for _ in 0..80 {
        counts_by_timer = simulate_day(counts_by_timer);
    }

    let part1_fish_count: i64 = counts_by_timer.iter().sum();
    println!("Part 1:\t{part1_fish_count}");

    for _ in 80..256 {
        counts_by_timer = simulate_day(counts_by_timer);
    }

    let part2_fish_count: i64 = counts_by_timer.iter().sum();
    println!("Part 2:\t{part2_fish_count}");
}

fn simulate_day(counts: [i64; 9]) -> [i64; 9] {
    [
        counts[1], // 0
        counts[2], // 1
        counts[3], // 2
        counts[4], // 3
        counts[5], // 4
        counts[6], // 5
        counts[7] + counts[0], // 6
        counts[8], // 7
        counts[0]  // 8 - newly spawn
    ]
}
