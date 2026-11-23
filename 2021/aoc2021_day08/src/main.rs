fn main() {
    let input = include_str!("../input.txt");
    //let input = include_str!("../input_sample.txt");
    //let input = "acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab |cdfeb fcadb cdfeb cdbaf";

    let easy_digit_count = input.lines()
        .flat_map(|line|
            line.split_once('|').unwrap().1.trim().split(' ').map(|segments| segments.trim().len()))
        .filter(|len| *len == 2 || *len == 4 || *len == 3 || *len == 7)
        .count();

    println!("Part 1: {easy_digit_count}");

    let input: Vec<(Vec<u8>, Vec<u8>)> = input.lines()
        .map(|line| {
            let (all_digits, value) = line.split_once('|').unwrap();

            let all_digits: Vec<u8> = all_digits.trim().split(' ').map(encode_to_mask).collect();
            let value: Vec<u8> = value.trim().split(' ').map(encode_to_mask).collect();

            (all_digits, value)
        })
        .collect();

    //let i0 = input.iter().next().unwrap();
    //println!("{:?}", i0);
    //let solved_digits = solve_digits(&(*i0).0);
    //println!("{:?}", solved_digits);
    //let solved_digits_ch: Vec<String> = solved_digits.map(decode_from_mask).into_iter().collect();
    //println!("{:?}", solved_digits_ch);

    let sum_of_outputs: i32 = input.iter().map(|i0| {
        let solved_digits = solve_digits(&(*i0).0);
        let actual_digits: Vec<usize> = i0.1.iter().map(|&digit| solved_digits.iter().position(|&x| x == digit).unwrap()).collect();
        //println!("{:?}", actual_digits);
        let value = actual_digits.iter().fold(0i32, |b, digit| b * 10 + (*digit as i32));
        value
    }).sum();

    println!("Part 2: {sum_of_outputs}");
}

fn encode_to_mask(segments: &str) -> u8 {
    let mask = (segments).chars().map(|ch| ch as u8 - b'a')
        .fold(0, |b, d| b | (1 << d));
    mask
}

fn decode_from_mask(mask: u8) -> String {
    let mut idx = 0;
    let mut result = String::new();
    let mut mask = mask;

    while mask > 0 {
        if (mask & 1) == 1 {
            result.push((('a' as u8) + idx) as char);
        }

        mask = mask >> 1;
        idx = idx + 1;
    }
    result
}

fn solve_digits(all_digits: &[u8]) -> [u8; 10] {
    let one = *all_digits.iter().filter(|digit| count_on_bits(**digit) == 2).next().unwrap();
    let four = *all_digits.iter().filter(|digit| count_on_bits(**digit) == 4).next().unwrap();
    let seven = *all_digits.iter().filter(|digit| count_on_bits(**digit) == 3).next().unwrap();
    let eight = *all_digits.iter().filter(|digit| count_on_bits(**digit) == 7).next().unwrap();

    let zero_and_six_and_nine: Vec<u8> = all_digits.iter().filter(|digit| count_on_bits(**digit) == 6).map(|x| *x).collect();
    let two_and_three_and_five: Vec<u8> = all_digits.iter().filter(|digit| count_on_bits(**digit) == 5).map(|x| *x).collect();

    let three = *two_and_three_and_five.iter().filter(|digit| count_on_bits(**digit & !one) == 3).next().unwrap();
    let two_and_five: Vec<u8> = two_and_three_and_five.iter().filter(|digit| **digit != three).map(|x| *x).collect();

    let six = *zero_and_six_and_nine.iter().filter(|digit| (**digit | one) != **digit).next().unwrap();
    let zero_and_nine: Vec<u8> = zero_and_six_and_nine.iter().filter(|digit| **digit != six).map(|x| *x).collect();

    let five = *two_and_five.iter().filter(|digit| **digit & six == **digit).next().unwrap();
    let two = *two_and_five.iter().filter(|digit| **digit != five).next().unwrap();

    let nine = *zero_and_nine.iter().filter(|digit| **digit & three == three).next().unwrap();
    let zero = *zero_and_nine.iter().filter(|digit| **digit != nine).next().unwrap();

    [zero, one, two, three, four, five, six, seven, eight, nine ]
}

fn count_on_bits(number: u8) -> usize {
    let mut x = number;
    let mut result: usize = 0;
    while x > 0 {
        if (x & 1) == 1 {
            result = result + 1;
        }
        x = x >> 1;
    }
    result
}