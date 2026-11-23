fn main() {
    let input = include_str!("../input.txt");
    //let input = include_str!("../input_sample.txt");

    let input : Vec<Vec<char>> = input.lines().map(|line| line.chars().collect()).collect();

    let invalid_score: i32 = input.iter().filter_map(syntax_check).map(evaluate_invalid_character).sum();

    println!("Part 1: {invalid_score}");

    let mut completion_scores: Vec<i64> = input.iter()
        .filter(|line| syntax_check(line) == None)
        .map(|line| evaluate_completion(complete(line)))
        .collect();
    completion_scores.sort();
    let med_completion_score = completion_scores[completion_scores.len() / 2];

    println!("Part 2: {med_completion_score}");
}

fn evaluate_invalid_character(ch: char) -> i32 {
    match ch {
        ')' => 3,
        ']' => 57,
        '}' => 1197,
        '>' => 25137,
        _ => panic!()
    }
}

fn evaluate_valid_character(ch: char) -> i64 {
    match close_for(ch) {
        ')' => 1,
        ']' => 2,
        '}' => 3,
        '>' => 4,
        _ => panic!("Invalid char '{ch}'")
    }
}

fn evaluate_completion(mut stack: Vec<char>) -> i64 {
    let mut score = 0;

    while stack.len() > 0 {
        let ch  =stack.pop().unwrap();
        score = 5 * score + evaluate_valid_character(ch);
    }

    score
}

fn syntax_check(line: &Vec<char>) -> Option<char> {
    let mut stack: Vec<char> = Vec::new();

    for (_idx, ch) in line.iter().enumerate() {
        if is_opening(*ch) {
            stack.push(*ch);
        }
        else if is_closing(*ch) {
            match stack.pop() {
                Some(opening) if does_match(opening, *ch) => { },
                Some(_opening) => { return Some(*ch); }
                None => {}
            }
        }
    }

    None
}

fn complete(line: &Vec<char>) -> Vec<char> {
    let mut stack: Vec<char> = Vec::new();

    for (_idx, ch) in line.iter().enumerate() {
        if is_opening(*ch) {
            stack.push(*ch);
        }
        else if is_closing(*ch) {
            match stack.pop() {
                Some(opening) if does_match(opening, *ch) => { },
                Some(_opening) => panic!("Invalid character"),
                None => { }
            }
        }
    }

    stack
}

fn is_opening(ch: char) -> bool {
    match ch {
        '(' => true,
        '[' => true,
        '{' => true,
        '<' => true,
        _ => false
    }
}

fn is_closing(ch: char) -> bool {
    match ch {
        ')' => true,
        ']' => true,
        '}' => true,
        '>' => true,
        _ => false
    }
}

fn does_match(opening: char, closing: char) -> bool {
    match opening {
        '(' => closing == ')',
        '[' => closing == ']',
        '{' => closing == '}',
        '<' => closing == '>',
        _ => panic!()
    }
}

fn close_for(ch: char) -> char {
    match ch {
        '(' => ')',
        '[' => ']',
        '{' => '}',
        '<' => '>',
        _ => panic!("Invalid char '{ch}'")
    }
}