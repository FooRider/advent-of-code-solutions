fn main() {
    //let input = include_str!("../input_sample.txt");
    let input = include_str!("../input.txt");

    let input: Vec<i32> = input.split(',').map(|x| x.parse().unwrap()).collect();

    let min = *input.iter().min().unwrap();
    let max = *input.iter().max().unwrap();

    let minimum_fuel_to_align = (min..=max).map(
        |target| {
            let fuel_consumed: i32 = input.iter().map(move |pos| (target - pos).abs()).sum();
            fuel_consumed
        })
        .min()
        .unwrap();

    println!("Part 1: {minimum_fuel_to_align}");

    let minimum_fuel_to_align2 = (min..=max).map(
        |target| {
            let fuel_consumed: i32 = input.iter().map(move |pos| fuel_cost_to_move((target - pos).abs())).sum();
            fuel_consumed
        })
        .min()
        .unwrap();

    println!("Part 2: {minimum_fuel_to_align2}");
}

fn fuel_cost_to_move(distance: i32) -> i32 {
    (distance * (distance + 1)) / 2
}