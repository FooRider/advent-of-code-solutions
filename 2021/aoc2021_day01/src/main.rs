use std::collections::VecDeque;

fn main() {
    //let input = include_str!("../input_sample.txt");
    let input = include_str!("../input.txt");

    let mut increases1 = 0;
    let mut increases2 = 0;
    let mut previous = i64::MAX;
    let mut q1 : VecDeque<i64> = VecDeque::new();
    let mut q2 : VecDeque<i64> = VecDeque::new();

    for depth in input.lines().map(|line| line.trim().parse::<i64>().unwrap())
    {
        if depth > previous {
            increases1 = increases1 + 1;
        }
        previous = depth;

        q2.push_front(depth);
        while q2.iter().count() > 3 { q2.pop_back(); }

        if q2.iter().count() == 3 && q1.iter().count() == 3 {
            if q2.iter().sum::<i64>() > q1.iter().sum::<i64>() {
                increases2 = increases2 + 1;
            }
        }

        q1.push_front(depth);
        while q1.iter().count() > 3 { q1.pop_back(); }
    }

    println!("Part 1 Increases: {}", increases1);
    println!("Part 2 Increases: {}", increases2);
}
